﻿using EatDomicile.Api.Dtos.User;

namespace EatDomicile.Api.Dtos.Order;

public class OrderDto
{
    public int? Id { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int Status { get; set; }

    public UserDto User { get; set; }
}
