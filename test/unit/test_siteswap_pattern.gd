extends GutTest


func test_is_valid_returns_true_if_pattern_is_valid():
	assert_true(SiteswapPattern.parse("0").is_valid())
	assert_true(SiteswapPattern.parse("3").is_valid())
	assert_true(SiteswapPattern.parse("Z").is_valid())
	assert_true(SiteswapPattern.parse("a").is_valid())
	assert_true(SiteswapPattern.parse("Z").is_valid())
	assert_true(SiteswapPattern.parse("345").is_valid())
	assert_true(SiteswapPattern.parse(" 3 4 5 ").is_valid())
	assert_true(SiteswapPattern.parse("123456789abcdefghijklmnopqrstuvwxyz").is_valid())


func test_parse_returns_null_if_parse_failed():
	assert_null(SiteswapPattern.parse("$"))


func test_is_valid_returns_false_if_pattern_is_invalid():
	assert_false(SiteswapPattern.parse("223").is_valid())
	assert_false(SiteswapPattern.parse("54321").is_valid())
	assert_false(SiteswapPattern.parse("123456789abcdefghijklmnopqrstuvwxy").is_valid())


func test_ball_num_returns_ball_num_of_pattern_if_pattern_is_valid():
	assert_eq(SiteswapPattern.parse("0").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("3").ball_num(), 3)
	assert_eq(SiteswapPattern.parse("Z").ball_num(), 35)
	assert_eq(SiteswapPattern.parse("345").ball_num(), 4)


func test_ball_num_returns_0_if_pattern_is_invalid():
	assert_eq(SiteswapPattern.parse("223").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("54321").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("123456789abcdefghijklmnopqrstuvwxy").ball_num(), 0)


func test_label_returns_label_of_siteswap():
	assert_eq(SiteswapPattern.parse("9aB").label(), "9AB")


