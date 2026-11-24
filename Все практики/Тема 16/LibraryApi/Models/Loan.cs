
namespace LibraryApi.Models;
public class Loan
{
     public int Id { get; set; }
    public int BookId{ get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; } =DateTime.UtcNow ;
}