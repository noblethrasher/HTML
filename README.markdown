##Introduction##
I always found the ASP.NET web controls to be too heavy while the MVC controls felt a bit anemic (and abuse too many language features for my taste).  This library allows for the creation of HTML in a more functional style while the library itself can be easily extended using a pure OOP style.

##Example Code##
   
Suppose we had a structure like the following:
       
	   var scoobies = new Dictionary<int, string>
            {
                {1, "Buffy Summers"},
                {2, "Dawn Summers"},
                {3, "Xander Harris"},
                {4, "Willow Rosenberg"},
                {5, "Rupert Giles"},
                {6, "Cordelia Chase"},
            };


If we want
    
	<ul>
	   <li><a href="/person/1">Buffy Summers</a></li>
	   <li><a href="/person/2">Dawn Summers</a></li>
	   <li><a href="/person/3">Xander Harris</a></li>
	...
	</ul>

Then we write something like the following:

     <%
       var ul_scoobs = from s in scoobies 
                         select new Anchor () { Href = "/person/" + s.Key }
								.Wrap (s.Value)
								.WrapIn (new ListItem())
								.WrapIn (new UnorderedList());
     %>

    <%= ul_scoobs %>
			
This minimizes the interaction between server side code and markup.