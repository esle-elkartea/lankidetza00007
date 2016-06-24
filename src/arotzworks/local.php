<?php
include ("oocommon/oocommon.php");
include ("widgets.php");
$r = new request();
error_reporting("E_ERROR");
class aworkspage extends page
{
	function aworkspage()
	{
		$this->page();
		$this->stylesheet="style.css";
		$this->__widgetfields[] = html::write("<script src=\"shortcuts.js\"></script><div id=\"header\"></div>");
		$t = new toolbar();
		$t->addWidget(new button("Oficina","index.php","page"));
		$t->addWidget(new button("Operador de Taller","taller.php","wrench"));
		$t->addWidget(new button("Operador de Almacen","almacen.php","car"));
		$this->__widgetfields[] = $t;
	}
}

class aworkstrackpage extends page
{
	function aworkstrackpage()
	{
		$this->page();
		$this->stylesheet="style.css";
		$this->__widgetfields[] = html::write("<script src=\"shortcuts.js\"></script><div id=\"header\"></div>");
		$this->__widgetfields[] = html::write("<h1>Seguimiento de Pedidos</h1>");
	}
}
