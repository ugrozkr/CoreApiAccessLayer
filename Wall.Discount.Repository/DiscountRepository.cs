using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wall.Discount.Entity;
using Wall.Discount.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;

namespace Wall.Discount.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly MongoDbContext _context = null;

        public DiscountRepository(IOptions<Settings> settings)
        {
            _context = new MongoDbContext(settings);
        }

        public async Task<IEnumerable<Entity.Discount>> GetAllDiscounts()
        {
            try
            {
                return await _context.Discounts.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Entity.Discount> GetDiscount(Guid id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Discounts
                                .Find(Discount => Discount.Id == id || Discount.InternalId == internalId)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Entity.Discount>> GetDiscount(string code)
        {
            try
            {
                var query = _context.Discounts.Find(Discount => Discount.Code.Contains(code));
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObjectId GetInternalId(Guid id)
        {
            if (!ObjectId.TryParse(id.ToString(), out ObjectId internalId))
                internalId = ObjectId.Empty;
            return internalId;
        }

        public async Task AddDiscount(Entity.Discount item)
        {
            try
            {
                await _context.Discounts.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveDiscount(Guid id)
        {
            try
            {
                DeleteResult actionResult = await _context.Discounts.DeleteOneAsync(
                     Builders<Entity.Discount>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDiscount(Guid id, int amount)
        {
            var filter = Builders<Entity.Discount>.Filter.Eq(s => s.Id, id);
            var update = Builders<Entity.Discount>.Update.Set(s => s.Amount, amount);
            try
            {
                UpdateResult actionResult = await _context.Discounts.UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDiscount(Guid id, Entity.Discount item)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.Discounts
                                                .ReplaceOneAsync(n => n.Id.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDiscountDocument(Guid id, string code, int amount)
        {
            var item = await GetDiscount(id) ?? new Entity.Discount();
            item.Code = code;
            item.Amount = amount;
            return await UpdateDiscount(id, item);
        }

        public async Task<bool> RemoveAllDiscounts()
        {
            try
            {
                DeleteResult actionResult = await _context.Discounts.DeleteManyAsync(new BsonDocument());
                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
