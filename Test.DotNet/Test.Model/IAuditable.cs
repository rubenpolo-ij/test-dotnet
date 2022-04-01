using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Model
{
    public interface IAuditable
    {
        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

    }
}
