<?php
include ("local.php");
$p = new aworkspage();
$p->addWidget(html::write("<h1>Gesti&oacute;n de Maderas</h1>"));
$app = new dataapp("maderas","maderas.php","");
$p->addWidget($app);
$p->draw();
