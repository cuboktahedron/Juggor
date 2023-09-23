extends PopupMenu

signal ball_coloring_changed(coloring: Core.BallColoring)

const MENU_ID_COLORING_BY_PATTERN = 1000
const MENU_ID_COLORING_BY_BALL_NO = 1001

@onready var coloring = $Coloring


func _ready():
	add_submenu_item("coloring", coloring.name)
	coloring.coloring_changed.connect(_on_coloring_color_changed)


func _on_coloring_color_changed(coloring: Core.BallColoring):
	ball_coloring_changed.emit(coloring)
