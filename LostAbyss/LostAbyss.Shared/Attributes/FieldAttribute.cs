using System;
using System.Collections.Generic;
using System.Text;

namespace LostAbyss.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldAttribute : Attribute
    {
        public int Order;

        public FieldAttribute(int order) : base()
        {
            this.Order = order;
        }
    }
}
