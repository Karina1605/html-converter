using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFormatter.LinkBuilder.Options;

public class LinkServiceOptions
{
    public const string SectionName = nameof(LinkServiceOptions);

    public string Url { get; set; }
}
