using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Map_Editor
{
    [Serializable()]
    [DefaultPropertyAttribute("Name")]
    public class LevelInformation
    {

        private string _levelname = "";
        private string _author = "";
        private string _email = "";
        private string _datemodified = "";
        private float _gal_red = 0.2f;
        private float _gal_green = 0.2f;
        private float _gal_blue = 0.2f;
        private float _gal_alpha = 0.2f;

        public int blockiterator = 0;
        public int itemiterator = 0;

        [CategoryAttribute("Level Information"), DescriptionAttribute("Level Name")]
        public string LevelName
        {
            get
            {
                return _levelname;
            }
            set
            {
                _levelname = value;
            }
        }

        [CategoryAttribute("Level Information"), DescriptionAttribute("Author")]
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
            }
        }

        [CategoryAttribute("Level Information"), DescriptionAttribute("Contact email")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        [CategoryAttribute("Level Information"), DescriptionAttribute("Date modified")]
        public string DateModified
        {
            get
            {
                return _datemodified;
            }
            set
            {
                _datemodified = value;
            }
        }

        [CategoryAttribute("Global ambient light"), DescriptionAttribute("Red")]
        public float Red
        {
            get
            {
                return _gal_red;
            }
            set
            {
                _gal_red = value;
            }
        }
        [CategoryAttribute("Global ambient light"), DescriptionAttribute("Green")]
        public float Green
        {
            get
            {
                return _gal_green;
            }
            set
            {
                _gal_green = value;
            }
        }
        [CategoryAttribute("Global ambient light"), DescriptionAttribute("Blue")]
        public float Blue
        {
            get
            {
                return _gal_blue;
            }
            set
            {
                _gal_blue = value;
            }
        }
        [CategoryAttribute("Global ambient light"), DescriptionAttribute("Alpha")]
        public float Alpha
        {
            get
            {
                return _gal_alpha;
            }
            set
            {
                _gal_alpha = value;
            }
        }

        public void Clear()
        {
            this._levelname = "";
            this._author = "";
            this._email = "";
            this._datemodified = "";
            this.blockiterator = 0;
            this.itemiterator = 0;
            this._gal_red = 0.2f;
            this._gal_green = 0.2f;
            this._gal_blue = 0.2f;
            this._gal_alpha = 1.0f;
        }

    }
}

