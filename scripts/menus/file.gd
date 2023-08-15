extends PopupMenu

const MENU_ID_QUIT = 0


func _on_id_pressed(id):
	if id == MENU_ID_QUIT:
		get_tree().quit()
