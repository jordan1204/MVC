//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonResource.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NewsContent { get; set; }
        public Nullable<int> FileId { get; set; }
        public System.DateTime DateTime { get; set; }
        public bool Published { get; set; }
        public string Abstract { get; set; }
    }
}
