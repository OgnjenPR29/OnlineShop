namespace ServiceLayer.DataBase.Order
{
    public class PlaceItemDto : IDTO
    {
        public int Quantity { get; set; }

        public long ArticleId { get; set; }
    }
}