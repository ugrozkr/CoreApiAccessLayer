﻿using System;

namespace Wall.Basket.Entity
{
    public class BasketLineMessage
    {
        public Guid BasketLineId { get; set; }
        public int Price { get; set; }
        public int TicketAmount { get; set; }
    }
}
