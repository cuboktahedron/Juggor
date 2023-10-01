extends PopupMenu

signal fix_size_changed(is_fix_size: bool)
signal ball_coloring_changed(coloring: Core.BallColoring)

const MENU_ID_FIX_SIZE = 0

const MENU_ID_COLORING_BY_PATTERN = 1000
const MENU_ID_COLORING_BY_BALL_NO = 1001

@onready var coloring = $Coloring


func _ready():
	add_submenu_item("Coloring", coloring.name)


func _on_id_pressed(id):
	if id == MENU_ID_FIX_SIZE:
		toggle_item_checked(id)
		fix_size_changed.emit(is_item_checked(id))


func _on_coloring_color_changed(coloring: Core.BallColoring):
	ball_coloring_changed.emit(coloring)
