//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rs_content.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SONG
    {
        public int ID_SONG { get; set; }
        public Nullable<int> ID_SINGER { get; set; }
        public Nullable<int> ID_ALBUM { get; set; }
        public Nullable<int> ID_GENRE { get; set; }
        public string NAME_SONG { get; set; }
        public string PATH_SONG { get; set; }
        public string PICTURE_SONG { get; set; }
        public Nullable<int> ID_COMPOSER { get; set; }
    
        public virtual ALBUM ALBUM { get; set; }
        public virtual COMPOSER COMPOSER { get; set; }
        public virtual GENRE GENRE { get; set; }
        public virtual SINGER SINGER { get; set; }
    }
}
