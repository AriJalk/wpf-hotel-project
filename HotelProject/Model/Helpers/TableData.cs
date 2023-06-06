namespace HotelProject.Model.Helpers
{
    /// <summary>
    /// Helper class to format object to SQL entry for table creation
    /// </summary>
    public class TableData
    {
        private string _field;

        public string Field
        {
            get { return _field; }
            set { _field = value; }
        }

        private string _fieldname;

        public string FieldName
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }


        public TableData(string value,string fieldname)
        {
            Field = value;
            FieldName = fieldname;
        }
    }
}
