# de.lkraemer.nemmet

this projects bases on the Nemmet project of deanebarker at https://github.com/deanebarker/Nemmet <br />
this library is ported from the .NET 4.5.2 (the old project from deanebarker) to a .NET Standard 2.0 library. <br />
you can use this library in all .NET projects like .NET Core, .NET Legacy, Xamarin, etc. <br />

[![Downloads](https://img.shields.io/nuget/dt/de.lkraemer.nemmet.svg?style=flat-square)](http://www.nuget.org/packages/de.lkraemer.nemmet/) [![NuGet](https://img.shields.io/nuget/v/de.lkraemer.nemmet.svg?style=flat-square)](http://nuget.org/packages/de.lkraemer.nemmet) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/90e02cc2542a462e9fa065a32e326f50)](https://app.codacy.com/app/lk-code/nemmet?utm_source=github.com&utm_medium=referral&utm_content=lk-code/nemmet&utm_campaign=Badge_Grade_Dashboard) [![License](https://img.shields.io/github/license/lk-code/nemmet.svg?style=flat-square)](https://github.com/lk-code/nemmet/blob/master/LICENSE)

## installation

install the lib from nuget: https://www.nuget.org/packages/de.lkraemer.nemmet

## usage

    var code = "#my-panel.panel>.heading{Title}+.content{Content}+.footer";

	// To get a nested List<NemmetTag>
    // NemmetTag has a recursive ToHtml() method
	var tags = NemmetTag.Parse(code);

    // To get the HTML as a string (which just concats the results of ToHtml())
    var html = NemmetParser.GetHtml(code)

Result:

    <div id="my-panel" class="panel">
      <div class="heading">
        Title
      </div>
      <div class="content">
        Content
      </div>
      <div class="footer"></div>
    </div>

## what works

Read the [Emmet syntax guide](http://docs.emmet.io/abbreviations/syntax/) for the basics.  Here the subset that Nemmet supports.

* Simple elements: `div`
* Nested elements: `parent>child`
* Sibling elements: `div1+div2`
* The "climb up" operator: `parent1>child^parent2`
* IDs: `div#id`
* Classes: `div.class1.class2`
* Attributes: `div[key=value]`
* Multiple attributes: `div[key1=value1 key2=value2]`
* Content `div{Some text}`
* Default tag naming (though, the defaults need more definition)

## what doesn't work

* Repeating elements and auto-numbering (why would you need this at runtime?)
* Parentheticals/grouping (though, this is likely not far off -- I have a theory for it)
* Style abbreviations (not hard, but low on the priority list)

## Nemmet

Nemmet is an homage to [Emmet](http://emmet.io/) (Nemmet = "Not Emmet"...get it?), the HTML expansion language.

I wanted something in (1) a single file, (2) pure C#, and (3) source that I could debug through.  It will never be as full-featured as Emmet. I'm hoping for maybe 75% on a good day.
