namespace CoreApiInNet.Model
{
    public class QueryAntiFlood
    {
        public int _AmountItems = 40;
        /*public int GetAmoutItems () { return AmountItems; }
        public void SetAmountItems(int SetAmountItems)
        {
            AmountItems=SetAmountItems;
        }*/
        public int AmountItems
        {
            get
            {
                return _AmountItems;
            }
            set
            {
                _AmountItems=value;
            }
        }
        public int StartIndex {  get; set; }
        public int PageNumber { get; set; }
    }
}
