using HotelProject.Model.Helpers;
using System.Collections.Generic;

namespace HotelProject.Model.Interfaces
{
    interface ISqlElement
    {

        /// <summary>
        /// List Field Names according to table requirement<br/>
        /// Usage: FieldName DataType SqlArgs<br/>
        /// 
        /// example: ID INT PRIMARY KEY NOT NULL
        /// </summary>
        /// <returns></returns>
        List<string> GetTableTemplate();
        /// <summary>
        /// List all values in same order as TableTemplate
        /// </summary>
        /// <returns>List of TableData<br>
        /// Values in pair of TableData (Field/Field Name)</returns>
        List<TableData> GetValues();
        string GetPrimaryKeyType();
        int GetPrimaryKey();
    }
}
