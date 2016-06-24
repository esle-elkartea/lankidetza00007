function ga(x,a)
{
	return x.getAttribute(a);
}

function cn(t)
{
	td = document.createElement('td');
	td.innerHTML ="&nbsp;" + t + "&nbsp;";
	return td;
}

function cno(o)
{
	td = document.createElement('td');
	td.style.textAlign="center";
	td.appendChild(o);
	return td;
}

function cnt(t)
{
	td = document.createElement('td');
	td.style.textAlign="center";
		td.innerHTML ="&nbsp;";
	if (t == "1")
		td.innerHTML ="<img src=\"icons/16x16/accept.png\" />";
	return td;
}


function ap(e,n)
{
	e.appendChild(n);
}

function gv(id)
{
	return document.getElementById(id).value;
}

function gsv(id)
{
	return document.getElementById(id).childNodes[document.getElementById(id).selectedIndex].value;
}

function gvr(id)
{
	return document.getElementById(id).checked ? "1" : "0";
}