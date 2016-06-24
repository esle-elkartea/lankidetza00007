<?php
include ("local.php");
$sf = new scaffold("pedidos_entrada");
$sf->open($r->id);
$sf->delete();
print_arr(errorobject::errorlist());
echo "ok";
?>