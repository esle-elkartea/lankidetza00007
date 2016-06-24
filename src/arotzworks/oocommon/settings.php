<?php
/*
Webalianza ooCommon NG PHP Framework
(c) 2005 Webalianza T.I. S.L.
Development sponsored and started by
Webalianza T.I. S.L.
Project Lead:   Luis Martin-Santos Garcia <luis@webalianza.com>

This file is part of ooCommon NG.

    ooCommon NG is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    ooCommon is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with ooCommon NG; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
    
    $Id: settings.php,v 1.3 2005/12/28 08:53:49 luis Exp $
*/    
	$config["classes"]["database"]["mysql_username"] = "root";
	$config["classes"]["database"]["mysql_password"] = "root";
	$config["classes"]["database"]["mysql_database"] = "arotzworks";
	$config["classes"]["database"]["mysql_hostname"] = "localhost:8889";
	$config["classes"]["database"]["rowlimit"] = 50;
	$config["classes"]["boolwidget"]["true"] = "Si";
	$config["classes"]["boolwidget"]["false"] = "No";
	$config["classes"]["date"]["locale"] = "es_ES.UTF8";	
	$config["classes"]["session"]["sessions_table"] ="sessions";
	$config["classes"]["session"]["sessions_objects_table"] = "sessions_objects";
	$config["classes"]["session"]["sessions_acl_table"] = "sessions_acl";
	$config["classes"]["session"]["users_table"] = "sessions_users";
	$config["classes"]["user"]["users_table"] = "sessions_users";
	$config["classes"]["datagrid"]["message_results"] = "resultados";
	$config["classes"]["datagrid"]["message_of"] = "de";
	$config["classes"]["datagrid"]["message_to"] = "a";
	$config["classes"]["datagrid"]["message_orderby"] = "a";
	$config["classes"]["datagrid"]["message_count"] = "Mostrar ";
	$config["classes"]["datagrid"]["message_filter"] = "filtrar";
	$config["classes"]["dataapp"]["message_insert_ok"] = "Objeto insertado con exito";
	$config["classes"]["dataapp"]["message_insert_failed"] = "Fall&oacute; la inserci&oacute;n del objeto";
	$config["classes"]["dataapp"]["message_update_ok"] = "Objeto modificado con &eacute;xito";
	$config["classes"]["dataapp"]["message_update_failed"] = "Fall&oacute; la modificaci&oacute;n del objeto";
	$config["classes"]["dataapp"]["message_delete_ok"] = "Objeto borrado con &eacute;xito";
	$config["classes"]["dataapp"]["message_delete_confirm"] = "¿Seguro que desea borrar el objeto?";
	$config["classes"]["dataapp"]["message_delete_failed"] = "Fall&oacute; el borrado del objeto";
	$config["classes"]["dataapp"]["button_add"] = "A&ntilde;adir";
	$config["classes"]["dataapp"]["button_update"] = "Modificar";
	$config["classes"]["dataapp"]["button_delete"] = "Borrar";
	$config["classes"]["dataapp"]["button_list"] = "Volver al listado";
	$config["classes"]["dataobjectapp"]["message_insert_ok"] = "Objeto insertado con exito";
	$config["classes"]["dataobjectapp"]["message_insert_failed"] = "Fall&oacute; la inserci&oacute;n del objeto";
	$config["classes"]["dataobjectapp"]["message_update_ok"] = "Objeto modificado con &eacute;xito";
	$config["classes"]["dataobjectapp"]["message_update_failed"] = "Fall&oacute; la modificaci&oacute;n del objeto";
	$config["classes"]["dataobjectapp"]["message_delete_ok"] = "Objeto borrado con &eacute;xito";
	$config["classes"]["dataobjectapp"]["message_delete_failed"] = "Fall&oacute; el borrado del objeto";
	$config["classes"]["dataobjectapp"]["button_add"] = "A&ntilde;adir";
	$config["classes"]["dataobjectapp"]["button_update"] = "Modificar";
	$config["classes"]["dataobjectapp"]["button_delete"] = "Borrar";
	$config["classes"]["dataobjectapp"]["button_list"] = "Volver al listado";
	$config["classes"]["login"]["username_caption"] = "Usuario";
	$config["classes"]["login"]["password_caption"] = "Contrase&ntilde;a";
	$config["classes"]["login"]["submit_caption"] = "Iniciar Sesi&oacute;n";
	$config["classes"]["login"]["logout_caption"] = "Cerrar Sesi&oacute;n";
	$config["classes"]["login"]["logged_in_caption"] = "Ha iniciado sesi&oacute;n como ";
	$config["classes"]["login"]["logged_in_realm_caption"] = "Aplicaci&oacute;n";
	$config["classes"]["login"]["realm_caption"] = "Iniciar sesi&oacute;n en Aplicaci&oacute;n";
	$config["classes"]["ooentity"]["invalid_data"] = "Dato Incorrecto:";
        $config["classes"]["form"]["submit_caption"] = "Enviar";
