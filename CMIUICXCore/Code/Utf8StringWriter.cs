﻿using System.IO;
using System.Text;

namespace CMIUICXCore.Code
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
