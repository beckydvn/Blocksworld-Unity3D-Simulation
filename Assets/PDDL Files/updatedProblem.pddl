(define (problem BLOCKS-10-0)
(:domain BLOCKS)
(:objects GREEN RED PINK PURPLE ORANGE BLACK CYAN WHITE BLUE YELLOW )
 
(:INIT (ONTABLE RED) (ONTABLE ORANGE) (ONTABLE YELLOW) (ONTABLE GREEN) (ONTABLE CYAN) (ONTABLE BLUE) (ONTABLE PURPLE) (ONTABLE PINK) (ONTABLE WHITE) (ONTABLE BLACK)(CLEAR RED) (CLEAR ORANGE) (CLEAR YELLOW) (CLEAR GREEN) (CLEAR CYAN) (CLEAR BLUE) (CLEAR PURPLE) (CLEAR PINK) (CLEAR WHITE) (CLEAR BLACK)(HANDEMPTY)) 
(:goal (AND (ONTABLE RED) (ON ORANGE RED) (ON YELLOW ORANGE) (ONTABLE GREEN) (ONTABLE CYAN) (ONTABLE BLUE) (ONTABLE PURPLE) (ONTABLE PINK) (ONTABLE WHITE) (ONTABLE BLACK)  (CLEAR YELLOW) (CLEAR GREEN) (CLEAR CYAN) (CLEAR BLUE) (CLEAR PURPLE) (CLEAR PINK) (CLEAR WHITE) (CLEAR BLACK)(HANDEMPTY))))