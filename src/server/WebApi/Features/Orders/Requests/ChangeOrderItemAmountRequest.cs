﻿namespace WebApi.Features.Orders.Requests;

public class ChangeOrderItemAmountRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
}
