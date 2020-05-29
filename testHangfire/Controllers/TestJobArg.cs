namespace testHangfire.Controllers
{
    public class TestJobArg
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}