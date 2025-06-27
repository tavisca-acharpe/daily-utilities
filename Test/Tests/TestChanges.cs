namespace Test.Tests
{
    public static class TestChanges
    {
        public static void TestingChanges()
        {
            var order = new List<Order> {
            new Order() { Id = "123", Index = 8, ItemIds = new List<string>() { "123", "345"} },
            new Order() { Id = "456", Index = 9, ItemIds = new List<string>() { "123", "345"} } ,
            new Order() { Id = "789", Index = 100 },
            new Order() { Id = "", Index = 101 },
            new Order() { Id = "", Index = 102},
            new Order() { Id = "789", Index = 103 },
            new Order() { Id = "789", Index = 104 } };

            var itemIds = new List<string>() { "345", "123" };

            var affectedItemCharges = order.Where(x => x.ItemIds != null && x.ItemIds.Count == 1000).ToList();

            if (affectedItemCharges?.Any() ?? false)
            {
                var pointsCharge = affectedItemCharges.FirstOrDefault();
            }

            var affectedItemCharges1 = order.Where(x => x.ItemIds != null && x.ItemIds.Count == itemIds.Count && x.ItemIds.All(x => itemIds.Contains(x)));

            var clientOrders = new List<Order> { new Order() { Id = "123", Index = 2 }, new Order() { Id = "777", Index = 1 }, new Order() { Id = "888", Index = 3 } };

            var objectsThatMatch = order.Select(item => item)?.Where(i => clientOrders.Any(ci => ci.Id == i.Id))?.ToList();

            var clientOrder = order?.OrderByDescending(o => o.Index)?.FirstOrDefault();
        }
    }
}