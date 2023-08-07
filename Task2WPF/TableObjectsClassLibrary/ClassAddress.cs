namespace TableObjectsClassLibrary
{

    /// <summary>
    /// Information about class in table and its address
    /// </summary>
    public class ClassAddress
    {
        private int _rowStartInclusive;
        private int _rowEndInclusive;
        private string _NameOfClass;

        public int RowStartInclusive { get => _rowStartInclusive; set => _rowStartInclusive = value; }
        public int RowEndInclusive { get => _rowEndInclusive; set => _rowEndInclusive = value; }
        public string NameOfClass { get => _NameOfClass; set => _NameOfClass = value; }
    }
}