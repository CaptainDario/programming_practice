%
%

% HINTS:
% - For subgrid_size(s), subgrids can be identified by labels 1..s*s
% - A cell (x,y) can be mapped to the subgrid labeled (((x-1)/s)*s + (y+s-1)/s)

number(1..9).
subgrid_size(3).

subgrid(X,Y,((X-1)/S)*S + (Y+S-1)/S) :- subgrid_size(S), X = 1..S*S, Y = 1..S*S.

% Your implementation please...

%Generate

1 { sudoku(X, Y, N) : number(N)} 1 :- number(X), number(Y), not initial(X, Y, _).
1 { sudoku(X, Y ,N) : number(X), number(Y), subgrid(X, Y, S)} 1 :- number(N), number(S).
1 { sudoku(X, Y, N) : number(X)} 1 :- number(N), number(Y).
1 { sudoku(X, Y, N) : number(Y)} 1 :- number(N), number(X).

%define
sudoku(X, Y, N):- number(N), initial(X, Y, N).


%test


#show sudoku/3.
