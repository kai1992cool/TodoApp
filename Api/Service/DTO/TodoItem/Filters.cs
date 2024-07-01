namespace Service.DTO.TodoItem
{
    public class Filters
    {
        public string Status { get; set; } = String.Empty;

        public DateTime date { get; set; } = DateTime.Now;
    }
}