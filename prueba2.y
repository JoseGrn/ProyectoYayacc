E	: E '+' F
	| F;

F: F '*' T
	| T;

T	: '(' E ')'
	| '0'
	| '1';