using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTML
{
    public static class HtmlUtils
    {
        public static T Wrap<T>(this T element, IEnumerable<Element> elements) where T : Element
        {
            element.AddChildren (elements);
            return element;
        }

        public static T Wrap<T>(this T element, Element child) where T : Element
        {
            element.AddChild (child);
            return element;
        }

        public static T Wrap<T>(this T element, string child) where T : Element
        {
            element.AddChild (new TextNode () { Text = child });
            return element;
        }

        public static T WrapIn<T>(this Element elem, T parent) where T : Element
        {
            parent.AddChild (elem);
            return parent;
        }

        public static T WrapIn<T>(this IEnumerable<Element> elems, T parent) where T : Element
        {
            parent.AddChildren (elems);
            return parent;
        }
    }

    public class HtmlAttribute
    {
        public string Name { get; private set; }
        public string Value { get; set; }

        public HtmlAttribute(string Name, string Value)
        {
            this.Value = Value;
            this.Name = Name;
        }

        public virtual bool HasValue
        {
            get
            {
                return Value != null && Value.Length > 0;
            }
        }

        public override string ToString()
        {
            return this.Name + "=" + "\"" + this.Value + "\"";
        }
    }

    public abstract class Style
    {
        public abstract string Name { get; }
        public string Value { get; set; }

        public Style() { }

        public Style(string Value)
        {
            this.Value = Value;
        }

        public override string ToString()
        {
            return this.Name + ":" + this.Value;
        }
    }

    public sealed class Color : Style
    {
        public Color(string Value) : base (Value) { }

        public int CssColor { get; set; }
        
        public override string Name
        {
            get { return "color"; }
        }
    }

    public sealed class Border : Style
    {
        public override string Name
        {
            get { return "border"; }
        }

        public int BorderWidth
        {
            get;
            set;
        }

        public string BorderStyle
        {
            get;
            set;
        }

        public int BorderColor
        {
            get;
            set;
        }
    }


    public abstract class Element : IEquatable<Element>
    {

        Guid _id = Guid.NewGuid ();

        protected readonly string _name;

        protected List<HtmlAttribute> attrs = new List<HtmlAttribute> ();
        protected List<Element> children = new List<Element> ();

        HtmlAttribute elem_id = new HtmlAttribute ("id", "");

        public Element(string name)
        {
            this._name = name;
            attrs.Add (elem_id);
        }

        public string ID
        {
            get
            {
                return this.elem_id.Value;
            }

            set
            {
                this.elem_id.Value = value;
            }
        }

        protected virtual bool CanHaveChildren
        {
            get
            {
                return true;
            }
        }

        public virtual void AddAttribute(HtmlAttribute attr)
        {
            attrs.Add (attr);            
        }

        public virtual void RemoveAttribute(HtmlAttribute attr)
        {
            attrs.Remove (attr);            
        }

        public virtual void AddChild(Element elem)
        {
            if (CanHaveChildren)
            {
                if (elem.children.Contains (this))
                    elem.children.Remove (this);

                children.Add (elem);
            }
        }

        public virtual void AddChild(string  s)
        {
            if (CanHaveChildren)
            {
                children.Add (new TextNode () { Text = s });
            }
        }

        public virtual void RemoveChild(Element elem)
        {
            if (CanHaveChildren)
            {
                children.Remove (elem);
            }
        }

        public virtual void AddChildren(IEnumerable<Element> elems)
        {
            if (CanHaveChildren)
            {
                children.AddRange (elems);
            }
        }

        public override string ToString()
        {
            var startTag = "<" + _name + (attrs.Count > 0 ? attrs.Where(a => a.HasValue).Select (a => a.ToString ()).Aggregate ("", (x, y) => x + " " + y) : "");

            if (CanHaveChildren)
            {
                var sb = new StringBuilder ();

                foreach (var child in children)
                {
                    sb.AppendLine (child.ToString ());
                }

                return startTag + ">" + sb.ToString () + "</" + _name + ">";
            }
            else
            {
                return startTag + "/>";
            }
        }

        public bool Equals(Element other)
        {
            return other._id == this._id;
        }
    }

    public sealed class Anchor : Element
    {

        HtmlAttribute _href = new HtmlAttribute ("href", "");
        
        public Anchor() : base("a")
        {
            this.attrs.Add (_href);
        }

        public string Href
        {
            get
            {
                return _href.Value;
            }

            set
            {
                _href.Value = value;
            }
        }       
    }


    public sealed class Image : Element
    {
        public Image() : base("img")
        {
            this.attrs.Add (_img);
        }

        protected override bool CanHaveChildren
        {
            get
            {
                return false;
            }
        }

        HtmlAttribute _img = new HtmlAttribute ("src", "");

        public string ImgSrc
        {
            get
            {
                return _img.Value;
            }
            set
            {
                _img.Value = value;
            }
        }
    }

    public sealed class Paragraph : Element
    {
        public Paragraph() : base ("p") { }  
    }


    public sealed class TextNode : Element
    {
        public string Text { get; set; }

        public TextNode() : base ("") { }


        /// <summary>
        /// This is a Noop
        /// </summary>
        /// <param name="attr"></param>
        public override void AddAttribute(HtmlAttribute attr)
        {
            
        }

        /// <summary>
        /// This is a Noop
        /// </summary>
        /// <param name="attr"></param>
        public override void AddChild(Element elem)
        {
            
        }

        /// <summary>
        /// This is a Noop
        /// </summary>
        /// <param name="attr"></param>
        public override void AddChild(string s)
        {
            
        }

        /// <summary>
        /// This is a Noop
        /// </summary>
        /// <param name="attr"></param>
        public override void AddChildren(IEnumerable<Element> elems)
        {
            
        }

        /// <summary>
        /// This is a Noop
        /// </summary>
        /// <param name="attr"></param>
        public override void RemoveAttribute(HtmlAttribute attr)
        {
            
        }

        /// <summary>
        /// This is a Noop
        /// </summary>
        /// <param name="attr"></param>
        public override void RemoveChild(Element elem)
        {
            
        }

        public override string ToString()
        {
            return this.Text;
        }

        public static implicit operator TextNode(string s)
        {
            return new TextNode () { Text = s };
        }
    }


    public sealed class Table : Element
    {
        public Table() : base ("table") { }
    }

    public sealed class TableRow : Element
    {
        public TableRow() : base ("tr") { }
    }

    public sealed class TableCell : Element
    {
       private TableCell() : base ("td") { }
        
        public TableCell(string s) : this()
        {
            this.children.Add (new TextNode () { Text = s });
        }
    }

    public sealed class ListItem : Element
    {
        public ListItem() : base ("li") { }        
    }

    public sealed class UnorderedList : Element
    {
        public UnorderedList() : base ("ul") { }
    }

    public sealed class OrderedList : Element
    {
        public OrderedList() : base ("ol") { }
    }
}
