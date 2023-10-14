class_name SiteswapFactor

var _ss_height: SiteswapHeight

var height:
	get: return _ss_height.height


func _init(height: SiteswapHeight):
	_ss_height = height


static func parse(context: SiteswapParseContext) -> SiteswapFactor:
	var c = context.peek()
	
	var height = SiteswapHeight.parse(context)
	if height == null:
		return null
	return SiteswapFactor.new(height)


func label():
	return _ss_height.label()
