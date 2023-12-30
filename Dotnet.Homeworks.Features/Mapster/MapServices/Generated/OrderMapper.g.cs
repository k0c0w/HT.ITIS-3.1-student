using System;
using System.Collections.Generic;
using System.Linq;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Mapster.Utils;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions
{
    public partial class OrderMapper : IOrderMapper
    {
        public GetOrderDto MapFromOrder(Order p1)
        {
            return p1 == null ? null : new GetOrderDto(p1.Id, p1.ProductsIds == null ? null : MapsterHelper.ToEnumerable<Guid>(p1.ProductsIds));
        }
        public GetOrdersDto MapFromOrders(IEnumerable<Order> p2)
        {
            return p2 == null ? null : new GetOrdersDto(p2 == null ? null : p2.Select<Order, GetOrderDto>(funcMain1));
        }
        
        private GetOrderDto funcMain1(Order p3)
        {
            return p3 == null ? null : new GetOrderDto(p3.Id, p3.ProductsIds == null ? null : MapsterHelper.ToEnumerable<Guid>(p3.ProductsIds));
        }
    }
}