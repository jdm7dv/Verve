/* A Bison parser, made by GNU Bison 2.4.2.  */

/* Skeleton implementation for Bison's Yacc-like parsers in C
   
      Copyright (C) 1984, 1989-1990, 2000-2006, 2009-2010 Free Software
   Foundation, Inc.
   
   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.
   
   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.
   
   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.  */

/* As a special exception, you may create a larger work that contains
   part or all of the Bison parser skeleton and distribute that work
   under terms of your choice, so long as that work isn't itself a
   parser generator using the skeleton or a modified version thereof
   as a parser skeleton.  Alternatively, if you modify or redistribute
   the parser skeleton itself, you may (at your option) remove this
   special exception, which will cause the skeleton and the resulting
   Bison output files to be licensed under the GNU General Public
   License without this special exception.
   
   This special exception was added by the Free Software Foundation in
   version 2.2 of Bison.  */

/* C LALR(1) parser skeleton written by Richard Stallman, by
   simplifying the original so-called "semantic" parser.  */

/* All symbols defined below should begin with yy or YY, to avoid
   infringing on user name space.  This should be done even for local
   variables, as they might otherwise be expanded by user macros.
   There are some unavoidable exceptions within include files to
   define necessary library symbols; they are noted "INFRINGES ON
   USER NAME SPACE" below.  */

/* Identify Bison output.  */
#define YYBISON 1

/* Bison version.  */
#define YYBISON_VERSION "2.4.2"

/* Skeleton name.  */
#define YYSKELETON_NAME "yacc.c"

/* Pure parsers.  */
#define YYPURE 0

/* Push parsers.  */
#define YYPUSH 0

/* Pull parsers.  */
#define YYPULL 1

/* Using locations.  */
#define YYLSP_NEEDED 0



/* Copy the first part of user declarations.  */

/* Line 189 of yacc.c  */
#line 24 "grammar.y"

#include <stdio.h>
#include <stdlib.h>
  
#include "strutils.h"
#include "telma.h"

#define COMMENT_COLUMN   40  // 
#define INDENTATION_SIZE 2   // number of spaces to indent output C code

/* macro to create indentation space */
#define INDENT (strpad(newstr(""), INDENTATION_SIZE, ' '))

/* Local function prototypes */
static void report_error(const char *expected_symbol);
static void report_warning(const char *expected_symbol);

/* Global variables local to this file */
static char *current_act = NULL;
static char *current_scene = NULL;
static int num_errors = 0;           // error counter
static int num_warnings = 0;         // warning counter
static int i;                        // all-purpose counter



/* Line 189 of yacc.c  */
#line 99 "grammar.tab.c"

/* Enabling traces.  */
#ifndef YYDEBUG
# define YYDEBUG 0
#endif

/* Enabling verbose error messages.  */
#ifdef YYERROR_VERBOSE
# undef YYERROR_VERBOSE
# define YYERROR_VERBOSE 1
#else
# define YYERROR_VERBOSE 0
#endif

/* Enabling the token table.  */
#ifndef YYTOKEN_TABLE
# define YYTOKEN_TABLE 0
#endif


/* Tokens.  */
#ifndef YYTOKENTYPE
# define YYTOKENTYPE
   /* Put the tokens into the symbol table, so that GDB and other debuggers
      know about them.  */
   enum yytokentype {
     ARTICLE = 258,
     BE = 259,
     CHARACTER = 260,
     FIRST_PERSON = 261,
     FIRST_PERSON_POSSESSIVE = 262,
     FIRST_PERSON_REFLEXIVE = 263,
     NEGATIVE_ADJECTIVE = 264,
     NEGATIVE_COMPARATIVE = 265,
     NEGATIVE_NOUN = 266,
     NEUTRAL_ADJECTIVE = 267,
     NEUTRAL_NOUN = 268,
     NOTHING = 269,
     POSITIVE_ADJECTIVE = 270,
     POSITIVE_COMPARATIVE = 271,
     POSITIVE_NOUN = 272,
     SECOND_PERSON = 273,
     SECOND_PERSON_POSSESSIVE = 274,
     SECOND_PERSON_REFLEXIVE = 275,
     THIRD_PERSON_POSSESSIVE = 276,
     COLON = 277,
     COMMA = 278,
     EXCLAMATION_MARK = 279,
     LEFT_BRACKET = 280,
     PERIOD = 281,
     QUESTION_MARK = 282,
     RIGHT_BRACKET = 283,
     AND = 284,
     AS = 285,
     ENTER = 286,
     EXEUNT = 287,
     EXIT = 288,
     HEART = 289,
     IF_NOT = 290,
     IF_SO = 291,
     LESS = 292,
     LET_US = 293,
     LISTEN_TO = 294,
     MIND = 295,
     MORE = 296,
     NOT = 297,
     OPEN = 298,
     PROCEED_TO = 299,
     RECALL = 300,
     REMEMBER = 301,
     RETURN_TO = 302,
     SPEAK = 303,
     THAN = 304,
     THE_CUBE_OF = 305,
     THE_DIFFERENCE_BETWEEN = 306,
     THE_FACTORIAL_OF = 307,
     THE_PRODUCT_OF = 308,
     THE_QUOTIENT_BETWEEN = 309,
     THE_REMAINDER_OF_THE_QUOTIENT_BETWEEN = 310,
     THE_SQUARE_OF = 311,
     THE_SQUARE_ROOT_OF = 312,
     THE_SUM_OF = 313,
     TWICE = 314,
     WE_MUST = 315,
     WE_SHALL = 316,
     ACT_ROMAN = 317,
     SCENE_ROMAN = 318,
     ROMAN_NUMBER = 319,
     NONMATCH = 320
   };
#endif



#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED
typedef union YYSTYPE
{

/* Line 214 of yacc.c  */
#line 50 "grammar.y"

  char *str;
  struct {
    int num;
    char **list;
  } stringlist;



/* Line 214 of yacc.c  */
#line 210 "grammar.tab.c"
} YYSTYPE;
# define YYSTYPE_IS_TRIVIAL 1
# define yystype YYSTYPE /* obsolescent; will be withdrawn */
# define YYSTYPE_IS_DECLARED 1
#endif


/* Copy the second part of user declarations.  */


/* Line 264 of yacc.c  */
#line 222 "grammar.tab.c"

#ifdef short
# undef short
#endif

#ifdef YYTYPE_UINT8
typedef YYTYPE_UINT8 yytype_uint8;
#else
typedef unsigned char yytype_uint8;
#endif

#ifdef YYTYPE_INT8
typedef YYTYPE_INT8 yytype_int8;
#elif (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
typedef signed char yytype_int8;
#else
typedef short int yytype_int8;
#endif

#ifdef YYTYPE_UINT16
typedef YYTYPE_UINT16 yytype_uint16;
#else
typedef unsigned short int yytype_uint16;
#endif

#ifdef YYTYPE_INT16
typedef YYTYPE_INT16 yytype_int16;
#else
typedef short int yytype_int16;
#endif

#ifndef YYSIZE_T
# ifdef __SIZE_TYPE__
#  define YYSIZE_T __SIZE_TYPE__
# elif defined size_t
#  define YYSIZE_T size_t
# elif ! defined YYSIZE_T && (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
#  include <stddef.h> /* INFRINGES ON USER NAME SPACE */
#  define YYSIZE_T size_t
# else
#  define YYSIZE_T unsigned int
# endif
#endif

#define YYSIZE_MAXIMUM ((YYSIZE_T) -1)

#ifndef YY_
# if defined YYENABLE_NLS && YYENABLE_NLS
#  if ENABLE_NLS
#   include <libintl.h> /* INFRINGES ON USER NAME SPACE */
#   define YY_(msgid) dgettext ("bison-runtime", msgid)
#  endif
# endif
# ifndef YY_
#  define YY_(msgid) msgid
# endif
#endif

/* Suppress unused-variable warnings by "using" E.  */
#if ! defined lint || defined __GNUC__
# define YYUSE(e) ((void) (e))
#else
# define YYUSE(e) /* empty */
#endif

/* Identity function, used to suppress warnings about constant conditions.  */
#ifndef lint
# define YYID(n) (n)
#else
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static int
YYID (int yyi)
#else
static int
YYID (yyi)
    int yyi;
#endif
{
  return yyi;
}
#endif

#if ! defined yyoverflow || YYERROR_VERBOSE

/* The parser invokes alloca or malloc; define the necessary symbols.  */

# ifdef YYSTACK_USE_ALLOCA
#  if YYSTACK_USE_ALLOCA
#   ifdef __GNUC__
#    define YYSTACK_ALLOC __builtin_alloca
#   elif defined __BUILTIN_VA_ARG_INCR
#    include <alloca.h> /* INFRINGES ON USER NAME SPACE */
#   elif defined _AIX
#    define YYSTACK_ALLOC __alloca
#   elif defined _MSC_VER
#    include <malloc.h> /* INFRINGES ON USER NAME SPACE */
#    define alloca _alloca
#   else
#    define YYSTACK_ALLOC alloca
#    if ! defined _ALLOCA_H && ! defined _STDLIB_H && (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
#     include <stdlib.h> /* INFRINGES ON USER NAME SPACE */
#     ifndef _STDLIB_H
#      define _STDLIB_H 1
#     endif
#    endif
#   endif
#  endif
# endif

# ifdef YYSTACK_ALLOC
   /* Pacify GCC's `empty if-body' warning.  */
#  define YYSTACK_FREE(Ptr) do { /* empty */; } while (YYID (0))
#  ifndef YYSTACK_ALLOC_MAXIMUM
    /* The OS might guarantee only one guard page at the bottom of the stack,
       and a page size can be as small as 4096 bytes.  So we cannot safely
       invoke alloca (N) if N exceeds 4096.  Use a slightly smaller number
       to allow for a few compiler-allocated temporary stack slots.  */
#   define YYSTACK_ALLOC_MAXIMUM 4032 /* reasonable circa 2006 */
#  endif
# else
#  define YYSTACK_ALLOC YYMALLOC
#  define YYSTACK_FREE YYFREE
#  ifndef YYSTACK_ALLOC_MAXIMUM
#   define YYSTACK_ALLOC_MAXIMUM YYSIZE_MAXIMUM
#  endif
#  if (defined __cplusplus && ! defined _STDLIB_H \
       && ! ((defined YYMALLOC || defined malloc) \
	     && (defined YYFREE || defined free)))
#   include <stdlib.h> /* INFRINGES ON USER NAME SPACE */
#   ifndef _STDLIB_H
#    define _STDLIB_H 1
#   endif
#  endif
#  ifndef YYMALLOC
#   define YYMALLOC malloc
#   if ! defined malloc && ! defined _STDLIB_H && (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
void *malloc (YYSIZE_T); /* INFRINGES ON USER NAME SPACE */
#   endif
#  endif
#  ifndef YYFREE
#   define YYFREE free
#   if ! defined free && ! defined _STDLIB_H && (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
void free (void *); /* INFRINGES ON USER NAME SPACE */
#   endif
#  endif
# endif
#endif /* ! defined yyoverflow || YYERROR_VERBOSE */


#if (! defined yyoverflow \
     && (! defined __cplusplus \
	 || (defined YYSTYPE_IS_TRIVIAL && YYSTYPE_IS_TRIVIAL)))

/* A type that is properly aligned for any stack member.  */
union yyalloc
{
  yytype_int16 yyss_alloc;
  YYSTYPE yyvs_alloc;
};

/* The size of the maximum gap between one aligned stack and the next.  */
# define YYSTACK_GAP_MAXIMUM (sizeof (union yyalloc) - 1)

/* The size of an array large to enough to hold all stacks, each with
   N elements.  */
# define YYSTACK_BYTES(N) \
     ((N) * (sizeof (yytype_int16) + sizeof (YYSTYPE)) \
      + YYSTACK_GAP_MAXIMUM)

/* Copy COUNT objects from FROM to TO.  The source and destination do
   not overlap.  */
# ifndef YYCOPY
#  if defined __GNUC__ && 1 < __GNUC__
#   define YYCOPY(To, From, Count) \
      __builtin_memcpy (To, From, (Count) * sizeof (*(From)))
#  else
#   define YYCOPY(To, From, Count)		\
      do					\
	{					\
	  YYSIZE_T yyi;				\
	  for (yyi = 0; yyi < (Count); yyi++)	\
	    (To)[yyi] = (From)[yyi];		\
	}					\
      while (YYID (0))
#  endif
# endif

/* Relocate STACK from its old location to the new one.  The
   local variables YYSIZE and YYSTACKSIZE give the old and new number of
   elements in the stack, and YYPTR gives the new location of the
   stack.  Advance YYPTR to a properly aligned location for the next
   stack.  */
# define YYSTACK_RELOCATE(Stack_alloc, Stack)				\
    do									\
      {									\
	YYSIZE_T yynewbytes;						\
	YYCOPY (&yyptr->Stack_alloc, Stack, yysize);			\
	Stack = &yyptr->Stack_alloc;					\
	yynewbytes = yystacksize * sizeof (*Stack) + YYSTACK_GAP_MAXIMUM; \
	yyptr += yynewbytes / sizeof (*yyptr);				\
      }									\
    while (YYID (0))

#endif

/* YYFINAL -- State number of the termination state.  */
#define YYFINAL  71
/* YYLAST -- Last index in YYTABLE.  */
#define YYLAST   1063

/* YYNTOKENS -- Number of terminals.  */
#define YYNTOKENS  66
/* YYNNTS -- Number of nonterminals.  */
#define YYNNTS  52
/* YYNRULES -- Number of rules.  */
#define YYNRULES  226
/* YYNRULES -- Number of states.  */
#define YYNSTATES  350

/* YYTRANSLATE(YYLEX) -- Bison symbol number corresponding to YYLEX.  */
#define YYUNDEFTOK  2
#define YYMAXUTOK   320

#define YYTRANSLATE(YYX)						\
  ((unsigned int) (YYX) <= YYMAXUTOK ? yytranslate[YYX] : YYUNDEFTOK)

/* YYTRANSLATE[YYLEX] -- Bison symbol number corresponding to YYLEX.  */
static const yytype_uint8 yytranslate[] =
{
       0,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     1,     2,     3,     4,
       5,     6,     7,     8,     9,    10,    11,    12,    13,    14,
      15,    16,    17,    18,    19,    20,    21,    22,    23,    24,
      25,    26,    27,    28,    29,    30,    31,    32,    33,    34,
      35,    36,    37,    38,    39,    40,    41,    42,    43,    44,
      45,    46,    47,    48,    49,    50,    51,    52,    53,    54,
      55,    56,    57,    58,    59,    60,    61,    62,    63,    64,
      65
};

#if YYDEBUG
/* YYPRHS[YYN] -- Index of the first RHS symbol of rule number YYN in
   YYRHS.  */
static const yytype_uint16 yyprhs[] =
{
       0,     0,     3,     6,     9,    14,    19,    24,    26,    28,
      30,    32,    34,    36,    38,    40,    45,    50,    55,    57,
      60,    64,    68,    70,    72,    74,    76,    79,    81,    83,
      85,    88,    91,    94,    97,    99,   101,   103,   108,   113,
     118,   123,   127,   132,   137,   142,   146,   150,   154,   158,
     161,   164,   168,   173,   178,   182,   186,   191,   196,   201,
     206,   210,   215,   220,   224,   228,   232,   236,   239,   242,
     245,   247,   249,   251,   253,   255,   259,   263,   267,   269,
     272,   275,   277,   280,   283,   285,   287,   289,   292,   295,
     299,   302,   306,   310,   314,   316,   319,   322,   324,   327,
     330,   332,   334,   336,   338,   340,   342,   348,   354,   360,
     366,   368,   372,   376,   380,   384,   388,   392,   395,   397,
     399,   402,   405,   410,   415,   420,   422,   426,   430,   432,
     435,   437,   442,   446,   452,   457,   462,   467,   471,   475,
     481,   487,   493,   499,   501,   503,   505,   508,   510,   512,
     514,   516,   518,   520,   522,   524,   526,   528,   530,   532,
     534,   536,   538,   540,   542,   544,   546,   548,   550,   552,
     554,   556,   558,   560,   562,   564,   566,   568,   570,   572,
     574,   576,   578,   580,   582,   584,   586,   588,   590,   592,
     594,   596,   598,   600,   602,   604,   606,   608,   610,   612,
     614,   616,   618,   620,   622,   625,   627,   629,   631,   633,
     635,   637,   639,   641,   643,   645,   647,   649,   651,   653,
     655,   657,   662,   665,   670,   675,   680
};

/* YYRHS -- A `-1'-separated list of the rules' RHS.  */
static const yytype_int8 yyrhs[] =
{
     108,     0,    -1,    68,   103,    -1,    67,   103,    -1,    62,
      22,    74,    79,    -1,    62,    22,    74,     1,    -1,    62,
       1,    74,    79,    -1,    15,    -1,    12,    -1,     9,    -1,
      51,    -1,    53,    -1,    54,    -1,    55,    -1,    58,    -1,
       5,    23,    74,    79,    -1,     1,    23,    74,    79,    -1,
       5,     1,    74,    79,    -1,    71,    -1,    72,    71,    -1,
       5,    29,     5,    -1,     5,    23,    73,    -1,   111,    -1,
       1,    -1,    89,    -1,    95,    -1,    42,    92,    -1,    92,
      -1,    36,    -1,    35,    -1,     3,   114,    -1,     7,   114,
      -1,    19,   114,    -1,    21,   114,    -1,    14,    -1,   100,
      -1,   110,    -1,    25,    31,     5,    28,    -1,    25,    31,
      73,    28,    -1,    25,    33,     5,    28,    -1,    25,    32,
      73,    28,    -1,    25,    32,    28,    -1,    25,    31,     1,
      28,    -1,    25,    33,     1,    28,    -1,    25,    32,     1,
      28,    -1,    25,     1,    28,    -1,    30,    69,    30,    -1,
      30,     1,    30,    -1,    30,    69,     1,    -1,    75,    49,
      -1,    75,     1,    -1,    93,    34,   110,    -1,    48,    19,
      40,   110,    -1,    39,    19,    34,   110,    -1,    93,    40,
     110,    -1,    93,     1,   110,    -1,    48,     1,    40,   110,
      -1,    39,     1,    34,   110,    -1,    48,    19,     1,   110,
      -1,    39,    19,     1,   110,    -1,    93,    34,     1,    -1,
      48,    19,    40,     1,    -1,    39,    19,    34,     1,    -1,
      93,    40,     1,    -1,    85,    62,   110,    -1,    85,    63,
     110,    -1,    85,     1,   110,    -1,    86,    87,    -1,     1,
      87,    -1,    86,     1,    -1,    38,    -1,    60,    -1,    61,
      -1,    44,    -1,    47,    -1,     5,    22,   107,    -1,     5,
      22,     1,    -1,     5,     1,   107,    -1,    10,    -1,    41,
       9,    -1,    37,    15,    -1,    91,    -1,     9,    90,    -1,
      12,    90,    -1,    11,    -1,    81,    -1,    82,    -1,    43,
      19,    -1,    43,     1,    -1,   113,    72,    67,    -1,    94,
      67,    -1,   113,    72,     1,    -1,   113,     1,    67,    -1,
       1,    72,    67,    -1,    16,    -1,    41,    15,    -1,    37,
       9,    -1,    97,    -1,    15,    96,    -1,    12,    96,    -1,
      13,    -1,    17,    -1,     6,    -1,     8,    -1,    18,    -1,
      20,    -1,     4,   117,    76,   117,   100,    -1,     4,     1,
      76,   117,   100,    -1,     4,   117,     1,   117,   100,    -1,
       4,   117,    76,     1,   100,    -1,    27,    -1,    45,   111,
     110,    -1,    45,     1,   110,    -1,    45,   111,     1,    -1,
      46,   117,   110,    -1,    46,     1,   110,    -1,    46,   117,
       1,    -1,   105,   104,    -1,    80,    -1,    88,    -1,   104,
      80,    -1,   104,    88,    -1,    63,    22,    74,    79,    -1,
      63,    22,    74,     1,    -1,    63,     1,    74,    79,    -1,
     116,    -1,    77,    23,   116,    -1,    77,     1,   116,    -1,
     106,    -1,   107,   106,    -1,    94,    -1,    18,     4,    78,
     110,    -1,    18,   114,   110,    -1,    18,     4,    81,   117,
     110,    -1,    18,     4,    78,     1,    -1,    18,     4,     1,
     110,    -1,    18,     1,    78,   110,    -1,    18,   114,     1,
      -1,    18,     1,   110,    -1,    18,     4,    81,   117,     1,
      -1,    18,     4,    81,     1,   110,    -1,    18,     4,     1,
     117,   110,    -1,    18,     1,    81,   117,   110,    -1,    24,
      -1,    26,    -1,   112,    -1,   111,   112,    -1,     3,    -1,
       4,    -1,     5,    -1,     6,    -1,     7,    -1,     8,    -1,
       9,    -1,    10,    -1,    11,    -1,    12,    -1,    13,    -1,
      14,    -1,    15,    -1,    16,    -1,    17,    -1,    18,    -1,
      19,    -1,    20,    -1,    21,    -1,    23,    -1,    29,    -1,
      30,    -1,    31,    -1,    32,    -1,    33,    -1,    34,    -1,
      35,    -1,    36,    -1,    37,    -1,    38,    -1,    39,    -1,
      40,    -1,    41,    -1,    42,    -1,    43,    -1,    44,    -1,
      45,    -1,    46,    -1,    47,    -1,    48,    -1,    49,    -1,
      50,    -1,    51,    -1,    52,    -1,    53,    -1,    54,    -1,
      55,    -1,    56,    -1,    57,    -1,    58,    -1,    59,    -1,
      60,    -1,    61,    -1,    62,    -1,    63,    -1,    64,    -1,
      65,    -1,   111,    79,    -1,    96,    -1,    90,    -1,    50,
      -1,    52,    -1,    56,    -1,    57,    -1,    59,    -1,    83,
      -1,    84,    -1,    99,    -1,   101,    -1,   102,    -1,   109,
      -1,     5,    -1,    78,    -1,    98,    -1,    70,   117,    29,
     117,    -1,   115,   117,    -1,    70,   117,    29,     1,    -1,
      70,   117,     1,   117,    -1,    70,     1,    29,   117,    -1,
     115,     1,    -1
};

/* YYRLINE[YYN] -- source line where rule number YYN was defined.  */
static const yytype_uint16 yyrline[] =
{
       0,   185,   185,   188,   193,   200,   207,   216,   219,   222,
     227,   234,   241,   248,   255,   264,   274,   280,   288,   294,
     302,   309,   317,   320,   326,   330,   336,   340,   345,   349,
     355,   359,   363,   367,   371,   377,   380,   385,   391,   402,
     408,   419,   425,   432,   439,   446,   454,   460,   466,   474,
     478,   484,   490,   497,   504,   510,   516,   523,   530,   537,
     544,   550,   557,   564,   572,   577,   583,   591,   594,   599,
     606,   609,   612,   617,   620,   625,   630,   636,   644,   647,
     650,   655,   659,   663,   669,   674,   677,   682,   687,   694,
     700,   703,   710,   716,   723,   726,   729,   734,   738,   742,
     748,   751,   756,   759,   762,   765,   770,   777,   785,   793,
     803,   808,   814,   820,   828,   833,   839,   846,   851,   854,
     857,   860,   865,   872,   879,   888,   891,   894,   900,   903,
     908,   930,   936,   941,   948,   954,   961,   967,   972,   978,
     985,   993,  1000,  1009,  1012,  1017,  1020,  1024,  1025,  1026,
    1027,  1028,  1029,  1030,  1031,  1032,  1033,  1034,  1035,  1036,
    1037,  1038,  1039,  1040,  1041,  1042,  1044,  1046,  1047,  1048,
    1049,  1050,  1051,  1052,  1053,  1054,  1055,  1056,  1057,  1058,
    1059,  1060,  1061,  1062,  1063,  1064,  1065,  1066,  1067,  1068,
    1069,  1070,  1071,  1072,  1073,  1074,  1075,  1076,  1077,  1078,
    1080,  1081,  1082,  1084,  1088,  1096,  1099,  1104,  1110,  1116,
    1122,  1128,  1136,  1139,  1142,  1145,  1148,  1151,  1156,  1159,
    1162,  1165,  1170,  1174,  1184,  1189,  1199
};
#endif

#if YYDEBUG || YYERROR_VERBOSE || YYTOKEN_TABLE
/* YYTNAME[SYMBOL-NUM] -- String name of the symbol SYMBOL-NUM.
   First, the terminals, then, starting at YYNTOKENS, nonterminals.  */
static const char *const yytname[] =
{
  "$end", "error", "$undefined", "ARTICLE", "BE", "CHARACTER",
  "FIRST_PERSON", "FIRST_PERSON_POSSESSIVE", "FIRST_PERSON_REFLEXIVE",
  "NEGATIVE_ADJECTIVE", "NEGATIVE_COMPARATIVE", "NEGATIVE_NOUN",
  "NEUTRAL_ADJECTIVE", "NEUTRAL_NOUN", "NOTHING", "POSITIVE_ADJECTIVE",
  "POSITIVE_COMPARATIVE", "POSITIVE_NOUN", "SECOND_PERSON",
  "SECOND_PERSON_POSSESSIVE", "SECOND_PERSON_REFLEXIVE",
  "THIRD_PERSON_POSSESSIVE", "COLON", "COMMA", "EXCLAMATION_MARK",
  "LEFT_BRACKET", "PERIOD", "QUESTION_MARK", "RIGHT_BRACKET", "AND", "AS",
  "ENTER", "EXEUNT", "EXIT", "HEART", "IF_NOT", "IF_SO", "LESS", "LET_US",
  "LISTEN_TO", "MIND", "MORE", "NOT", "OPEN", "PROCEED_TO", "RECALL",
  "REMEMBER", "RETURN_TO", "SPEAK", "THAN", "THE_CUBE_OF",
  "THE_DIFFERENCE_BETWEEN", "THE_FACTORIAL_OF", "THE_PRODUCT_OF",
  "THE_QUOTIENT_BETWEEN", "THE_REMAINDER_OF_THE_QUOTIENT_BETWEEN",
  "THE_SQUARE_OF", "THE_SQUARE_ROOT_OF", "THE_SUM_OF", "TWICE", "WE_MUST",
  "WE_SHALL", "ACT_ROMAN", "SCENE_ROMAN", "ROMAN_NUMBER", "NONMATCH",
  "$accept", "Act", "ActHeader", "Adjective", "BinaryOperator",
  "CharacterDeclaration", "CharacterDeclarationList", "CharacterList",
  "Comment", "Comparative", "Comparison", "Conditional", "Constant",
  "EndSymbol", "EnterExit", "Equality", "Inequality", "InOut", "Jump",
  "JumpPhrase", "JumpPhraseBeginning", "JumpPhraseEnd", "Line",
  "NegativeComparative", "NegativeConstant", "NegativeNoun",
  "NonnegatedComparison", "OpenYour", "Play", "PositiveComparative",
  "PositiveConstant", "PositiveNoun", "Pronoun", "Question",
  "QuestionSymbol", "Recall", "Remember", "Scene", "SceneContents",
  "SceneHeader", "Sentence", "SentenceList", "StartSymbol", "Statement",
  "StatementSymbol", "String", "StringSymbol", "Title",
  "UnarticulatedConstant", "UnaryOperator", "UnconditionalSentence",
  "Value", 0
};
#endif

# ifdef YYPRINT
/* YYTOKNUM[YYLEX-NUM] -- Internal token number corresponding to
   token YYLEX-NUM.  */
static const yytype_uint16 yytoknum[] =
{
       0,   256,   257,   258,   259,   260,   261,   262,   263,   264,
     265,   266,   267,   268,   269,   270,   271,   272,   273,   274,
     275,   276,   277,   278,   279,   280,   281,   282,   283,   284,
     285,   286,   287,   288,   289,   290,   291,   292,   293,   294,
     295,   296,   297,   298,   299,   300,   301,   302,   303,   304,
     305,   306,   307,   308,   309,   310,   311,   312,   313,   314,
     315,   316,   317,   318,   319,   320
};
# endif

/* YYR1[YYN] -- Symbol number of symbol that rule YYN derives.  */
static const yytype_uint8 yyr1[] =
{
       0,    66,    67,    67,    68,    68,    68,    69,    69,    69,
      70,    70,    70,    70,    70,    71,    71,    71,    72,    72,
      73,    73,    74,    74,    75,    75,    76,    76,    77,    77,
      78,    78,    78,    78,    78,    79,    79,    80,    80,    80,
      80,    80,    80,    80,    80,    80,    81,    81,    81,    82,
      82,    83,    83,    83,    83,    83,    83,    83,    83,    83,
      83,    83,    83,    83,    84,    84,    84,    85,    85,    85,
      86,    86,    86,    87,    87,    88,    88,    88,    89,    89,
      89,    90,    90,    90,    91,    92,    92,    93,    93,    94,
      94,    94,    94,    94,    95,    95,    95,    96,    96,    96,
      97,    97,    98,    98,    98,    98,    99,    99,    99,    99,
     100,   101,   101,   101,   102,   102,   102,   103,   104,   104,
     104,   104,   105,   105,   105,   106,   106,   106,   107,   107,
     108,   109,   109,   109,   109,   109,   109,   109,   109,   109,
     109,   109,   109,   110,   110,   111,   111,   112,   112,   112,
     112,   112,   112,   112,   112,   112,   112,   112,   112,   112,
     112,   112,   112,   112,   112,   112,   112,   112,   112,   112,
     112,   112,   112,   112,   112,   112,   112,   112,   112,   112,
     112,   112,   112,   112,   112,   112,   112,   112,   112,   112,
     112,   112,   112,   112,   112,   112,   112,   112,   112,   112,
     112,   112,   112,   112,   113,   114,   114,   115,   115,   115,
     115,   115,   116,   116,   116,   116,   116,   116,   117,   117,
     117,   117,   117,   117,   117,   117,   117
};

/* YYR2[YYN] -- Number of symbols composing right hand side of rule YYN.  */
static const yytype_uint8 yyr2[] =
{
       0,     2,     2,     2,     4,     4,     4,     1,     1,     1,
       1,     1,     1,     1,     1,     4,     4,     4,     1,     2,
       3,     3,     1,     1,     1,     1,     2,     1,     1,     1,
       2,     2,     2,     2,     1,     1,     1,     4,     4,     4,
       4,     3,     4,     4,     4,     3,     3,     3,     3,     2,
       2,     3,     4,     4,     3,     3,     4,     4,     4,     4,
       3,     4,     4,     3,     3,     3,     3,     2,     2,     2,
       1,     1,     1,     1,     1,     3,     3,     3,     1,     2,
       2,     1,     2,     2,     1,     1,     1,     2,     2,     3,
       2,     3,     3,     3,     1,     2,     2,     1,     2,     2,
       1,     1,     1,     1,     1,     1,     5,     5,     5,     5,
       1,     3,     3,     3,     3,     3,     3,     2,     1,     1,
       2,     2,     4,     4,     4,     1,     3,     3,     1,     2,
       1,     4,     3,     5,     4,     4,     4,     3,     3,     5,
       5,     5,     5,     1,     1,     1,     2,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     2,     1,     1,     1,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     4,     2,     4,     4,     4,     2
};

/* YYDEFACT[STATE-NAME] -- Default rule to reduce with in state
   STATE-NUM when YYTABLE doesn't specify something else to do.  Zero
   means the default is an error.  */
static const yytype_uint8 yydefact[] =
{
       0,     0,   147,   148,   149,   150,   151,   152,   153,   154,
     155,   156,   157,   158,   159,   160,   161,   162,   163,   164,
     165,   166,   167,   168,   169,   170,   171,   172,   173,   174,
     175,   176,   177,   178,   179,   180,   181,   182,   183,   184,
     185,   186,   187,   188,   189,   190,   191,   192,   193,   194,
     195,   196,   197,   198,   199,   200,   201,   202,   203,   130,
       0,     0,   145,     0,     0,     0,    18,     0,     0,    90,
       0,     1,   143,   144,   110,   204,    35,    36,   146,     0,
       0,     0,     0,     0,    93,    19,     0,     0,     0,     3,
       0,     2,    92,    91,    89,    23,     0,    22,     0,     0,
       0,     0,     0,     0,     0,     0,   118,   119,   117,    16,
      17,    15,     6,     5,     4,     0,     0,     0,     0,     0,
       0,     0,     0,   120,   121,   124,   123,   122,     0,     0,
       0,    29,    28,    70,     0,     0,     0,     0,     0,    71,
      72,     0,   212,   213,     0,     0,     0,   214,   215,   216,
     128,     0,   217,   125,    76,     0,    45,     0,     0,     0,
       0,     0,    41,     0,     0,     0,    73,    74,    68,     0,
       0,   218,   102,     0,   103,    34,   104,     0,   105,     0,
     207,    10,   208,    11,    12,    13,   209,   210,    14,   211,
       0,   219,   220,     0,     0,     0,     0,     0,    84,     0,
     100,     0,   101,   206,    81,   205,    97,     0,     0,     0,
      88,    87,     0,     0,     0,     0,     0,     0,     0,     0,
       0,     0,     0,    69,    67,     0,     0,     0,   129,    42,
       0,    37,     0,    38,    44,    40,    43,    39,    78,    94,
       0,     0,     0,     0,     0,     0,    85,    86,    24,    27,
      25,    30,    31,    32,    33,     0,     0,   226,   222,     0,
       0,     0,     0,   138,     0,     0,     0,     0,    82,    83,
      99,     0,    98,   137,   132,     0,     0,     0,   112,   113,
     111,   115,   116,   114,     0,     0,     0,   127,   126,    66,
      64,    65,    55,    60,    51,    63,    54,    21,    20,     0,
       9,     8,     7,     0,    96,    80,    79,    95,    26,    50,
      49,     0,     0,     0,     0,     0,     0,     0,   136,     0,
     135,     0,   134,   131,     0,     0,    57,    59,    62,    53,
      56,    58,    61,    52,    47,    48,    46,   107,   225,   224,
     223,   221,   108,   109,   106,   142,   141,   140,   139,   133
};

/* YYDEFGOTO[NTERM-NUM].  */
static const yytype_int16 yydefgoto[] =
{
      -1,    69,    70,   303,   190,    66,    67,   159,    96,   244,
     245,   141,   191,    75,   106,   246,   247,   142,   143,   144,
     145,   168,   107,   248,   203,   204,   249,   146,    59,   250,
     205,   206,   192,   147,    76,   148,   149,    89,   108,    90,
     150,   151,    60,   152,    77,    97,    62,    63,   207,   193,
     153,   194
};

/* YYPACT[STATE-NUM] -- Index in YYTABLE of the portion describing
   STATE-NUM.  */
#define YYPACT_NINF -195
static const yytype_int16 yypact[] =
{
     242,   119,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,   -36,
      28,   499,  -195,   263,    20,    40,  -195,    10,    32,    52,
      52,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,    -3,
      13,   307,   307,   307,    52,  -195,   307,   307,    38,  -195,
      46,  -195,    52,    20,    52,  -195,   625,   562,   625,   625,
     625,    75,   307,   307,    99,    65,  -195,  -195,    46,  -195,
    -195,  -195,  -195,  -195,  -195,   625,   118,   113,   627,   111,
     265,    37,   328,  -195,  -195,  -195,  -195,  -195,   401,   688,
     440,  -195,  -195,  -195,    90,   117,   372,   709,   131,  -195,
    -195,    80,  -195,  -195,     5,    11,    12,  -195,  -195,  -195,
    -195,    44,  -195,  -195,   401,   438,  -195,   143,   612,   422,
     426,   176,  -195,   433,   441,   450,  -195,  -195,  -195,   127,
     819,  -195,  -195,   819,  -195,  -195,  -195,   819,  -195,   819,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
     768,  -195,  -195,   789,   125,   629,   461,   649,  -195,   819,
    -195,   740,  -195,  -195,  -195,  -195,  -195,    87,   112,    16,
    -195,  -195,   370,   177,   370,   139,   358,    24,   638,   638,
     370,   370,   370,  -195,  -195,   370,   146,   178,  -195,  -195,
     395,  -195,   484,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
     478,   101,   145,   430,     8,  1004,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,   468,    18,  -195,  -195,  1004,
     848,   370,  1004,  -195,   947,   243,   869,   649,  -195,  -195,
    -195,   740,  -195,  -195,  -195,   370,   370,   308,  -195,  -195,
    -195,  -195,  -195,  -195,   370,   370,   373,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,   491,
    -195,  -195,  -195,    31,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,   497,  1004,  1004,   928,   497,   497,   497,  -195,   370,
    -195,   370,  -195,  -195,   370,   446,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,
    -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195,  -195
};

/* YYPGOTO[NTERM-NUM].  */
static const yytype_int16 yypgoto[] =
{
    -195,   567,  -195,  -195,  -195,   -40,   464,  -118,   618,  -195,
     392,  -195,   270,   635,   479,   300,  -195,  -195,  -195,  -195,
    -195,   485,   521,  -195,  -189,  -195,   390,  -195,  -195,  -195,
    -194,  -195,  -195,  -195,   273,  -195,  -195,   568,  -195,  -195,
     180,   519,  -195,  -195,  -191,     2,   -60,  -195,   315,  -195,
     240,  -137
};

/* YYTABLE[YYPACT[STATE-NUM]].  What to do in state STATE-NUM.  If
   positive, shift that token.  If negative, reduce the rule which
   number is the opposite.  If zero, do what YYDEFACT says.
   If YYTABLE_NINF, syntax error.  */
#define YYTABLE_NINF -78
static const yytype_int16 yytable[] =
{
     215,    78,    61,   163,   263,   270,   220,   272,   268,   309,
     269,    64,   223,   225,    93,    65,   274,   276,    65,   313,
      81,   278,   280,   281,   283,   285,    68,    85,    71,   289,
     290,   291,   335,    86,   292,   294,   296,    78,   160,   102,
      85,    82,   161,    81,   -77,   128,   226,   314,   129,   -77,
     277,   104,   227,   256,    87,   166,   258,   310,   167,    68,
     103,   336,   130,    83,   286,   162,   119,   221,   222,   -77,
     318,   105,    68,   320,   323,    68,   113,   270,   269,   131,
     132,   218,   133,   134,   326,   327,   329,   135,   273,   136,
     137,   208,   138,   330,   331,   333,   120,   121,   122,    72,
     117,    73,    74,   219,   139,   140,   -77,   -77,   311,   209,
     304,    72,   297,    73,   128,    88,   305,   129,   210,   126,
      64,   118,   315,   317,    65,   319,   259,   321,   345,   325,
     346,   130,   216,   347,   349,   238,   211,   238,   213,   156,
     282,   239,    72,   239,    73,    74,   275,   293,   131,   132,
     217,   133,   134,    78,   306,   240,   135,   240,   136,   137,
     307,   138,   241,    72,   241,    73,   242,   243,   242,   243,
      72,   229,    73,   139,   140,   338,   339,   341,   279,   295,
       2,     3,     4,     5,     6,     7,     8,     9,    10,    11,
      12,    13,    14,    15,    16,    17,    18,    19,    20,   230,
      21,    72,    72,    73,    73,   232,    22,    23,    24,    25,
      26,    27,    28,    29,    30,    31,    32,    33,    34,    35,
      36,    37,    38,    39,    40,    41,    42,    43,    44,    45,
      46,    47,    48,    49,    50,    51,    52,    53,    54,    55,
      56,    57,    58,     1,   322,     2,     3,     4,     5,     6,
       7,     8,     9,    10,    11,    12,    13,    14,    15,    16,
      17,    18,    19,    20,    79,    21,   157,    72,    65,    73,
     158,    22,    23,    24,    25,    26,    27,    28,    29,    30,
      31,    32,    33,    34,    35,    36,    37,    38,    39,    40,
      41,    42,    43,    44,    45,    46,    47,    48,    49,    50,
      51,    52,    53,    54,    55,    56,    57,    58,    95,   328,
       2,     3,     4,     5,     6,     7,     8,     9,    10,    11,
      12,    13,    14,    15,    16,    17,    18,    19,    20,   164,
      21,   228,    72,   165,    73,   228,    22,    23,    24,    25,
      26,    27,    28,    29,    30,    31,    32,    33,    34,    35,
      36,    37,    38,    39,    40,    41,    42,    43,    44,    45,
      46,    47,    48,    49,    50,    51,    52,    53,    54,    55,
      56,    57,    58,   212,   332,     2,     3,     4,     5,     6,
       7,     8,     9,    10,    11,    12,    13,    14,    15,    16,
      17,    18,    19,    20,    72,    21,    73,    72,   284,    73,
     161,    22,    23,    24,    25,    26,    27,    28,    29,    30,
      31,    32,    33,    34,    35,    36,    37,    38,    39,    40,
      41,    42,    43,    44,    45,    46,    47,    48,    49,    50,
      51,    52,    53,    54,    55,    56,    57,    58,   -75,   128,
     238,   195,   129,   -75,   196,   166,   239,   348,   167,   197,
     233,   198,   199,   200,   234,   201,   130,   202,   287,   288,
     240,   235,   264,   -75,   170,   261,   265,   241,   173,   236,
      72,   242,    73,   131,   132,   175,   133,   134,   237,   299,
     177,   135,   179,   136,   137,   251,   138,   300,   252,   298,
     301,   240,   253,   302,   254,   262,   266,   312,   139,   140,
     -75,   -75,     2,     3,     4,     5,     6,     7,     8,     9,
      10,    11,    12,    13,    14,    15,    16,    17,    18,    19,
      20,   334,    21,    72,    74,    73,    74,    80,    22,    23,
      24,    25,    26,    27,    28,    29,    30,    31,    32,    33,
      34,    35,    36,    37,    38,    39,    40,    41,    42,    43,
      44,    45,    46,    47,    48,    49,    50,    51,    52,    53,
      54,    55,    56,    57,    58,     2,     3,     4,     5,     6,
       7,     8,     9,    10,    11,    12,    13,    14,    15,    16,
      17,    18,    19,    20,   337,    21,   260,   123,   342,   343,
     344,    22,    23,    24,    25,    26,    27,    28,    29,    30,
      31,    32,    33,    34,    35,    36,    37,    38,    39,    40,
      41,    42,    43,    44,    45,    46,    47,    48,    49,    50,
      51,    52,    53,    54,    55,    56,    57,    58,   154,   124,
     224,   129,   170,   308,    84,   230,   173,   155,    91,   128,
     231,   232,   129,   175,     0,   130,    92,    94,   177,    72,
     179,    73,    74,    72,     0,    73,   130,     0,   197,   240,
     198,   267,   131,   132,     0,   133,   134,     0,     0,     0,
     135,     0,   136,   137,     0,   138,   133,   134,     0,     0,
       0,   135,     0,   136,   137,     0,   138,   139,   140,   169,
       0,   170,     0,   171,   172,   173,   174,     0,   139,   140,
      98,    99,   175,     0,   100,   101,   176,   177,   178,   179,
     214,     0,   170,     0,   171,   172,   173,   174,     0,     0,
     115,   116,     0,   175,     0,     0,     0,   176,   177,   178,
     179,   109,     0,   110,   111,   112,   114,     0,   180,   181,
     182,   183,   184,   185,   186,   187,   188,   189,     0,     0,
     125,   127,   271,   200,     0,   201,     0,   202,     0,   180,
     181,   182,   183,   184,   185,   186,   187,   188,   189,   255,
       0,   170,     0,   171,   172,   173,   174,     0,     0,     0,
       0,     0,   175,     0,     0,     0,   176,   177,   178,   179,
     257,     0,   170,     0,   171,   172,   173,   174,     0,     0,
       0,     0,     0,   175,     0,     0,     0,   176,   177,   178,
     179,     0,     0,     0,     0,     0,     0,     0,   180,   181,
     182,   183,   184,   185,   186,   187,   188,   189,   197,     0,
     198,   199,   200,     0,   201,     0,   202,     0,     0,   180,
     181,   182,   183,   184,   185,   186,   187,   188,   189,   316,
       0,   170,     0,   171,   172,   173,   174,     0,     0,     0,
       0,     0,   175,     0,     0,     0,   176,   177,   178,   179,
     324,     0,   170,     0,   171,   172,   173,   174,     0,     0,
       0,     0,     0,   175,     0,     0,     0,   176,   177,   178,
     179,     0,     0,     0,     0,     0,     0,     0,   180,   181,
     182,   183,   184,   185,   186,   187,   188,   189,     0,     0,
       0,     0,     0,     0,     0,     0,     0,     0,     0,   180,
     181,   182,   183,   184,   185,   186,   187,   188,   189,   340,
       0,   170,     0,   171,   172,   173,   174,     0,     0,     0,
       0,     0,   175,     0,     0,     0,   176,   177,   178,   179,
     170,     0,   171,   172,   173,   174,     0,     0,     0,     0,
       0,   175,     0,     0,     0,   176,   177,   178,   179,     0,
       0,    72,     0,    73,     0,     0,     0,     0,   180,   181,
     182,   183,   184,   185,   186,   187,   188,   189,     0,     0,
       0,     0,     0,     0,     0,     0,     0,   180,   181,   182,
     183,   184,   185,   186,   187,   188,   189,   170,     0,   171,
     172,   173,   174,     0,     0,     0,     0,     0,   175,     0,
       0,     0,   176,   177,   178,   179,     0,     0,     0,     0,
       0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
       0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
       0,     0,     0,     0,   180,   181,   182,   183,   184,   185,
     186,   187,   188,   189
};

static const yytype_int16 yycheck[] =
{
     137,    61,     0,   121,   195,   199,     1,   201,   197,     1,
     199,     1,     1,     1,     1,     5,   207,     1,     5,     1,
      23,   212,   213,   214,   215,     1,    62,    67,     0,   220,
     221,   222,     1,     1,   225,   226,   227,    97,     1,     1,
      80,     1,     5,    23,     0,     1,    34,    29,     4,     5,
      34,     5,    40,   190,    22,    44,   193,    49,    47,    62,
      22,    30,    18,    23,    40,    28,     1,    62,    63,    25,
     261,    25,    62,   264,   265,    62,     1,   271,   267,    35,
      36,     1,    38,    39,   275,   276,   277,    43,     1,    45,
      46,     1,    48,   284,   285,   286,    31,    32,    33,    24,
       1,    26,    27,    23,    60,    61,    62,    63,   245,    19,
       9,    24,   230,    26,     1,    63,    15,     4,     1,     1,
       1,    22,   259,   260,     5,   262,     1,   264,   319,   266,
     321,    18,     1,   324,   325,    10,    19,    10,   136,    28,
       1,    16,    24,    16,    26,    27,    34,     1,    35,    36,
      19,    38,    39,   213,     9,    30,    43,    30,    45,    46,
      15,    48,    37,    24,    37,    26,    41,    42,    41,    42,
      24,    28,    26,    60,    61,   312,   313,   314,     1,     1,
       3,     4,     5,     6,     7,     8,     9,    10,    11,    12,
      13,    14,    15,    16,    17,    18,    19,    20,    21,    23,
      23,    24,    24,    26,    26,    29,    29,    30,    31,    32,
      33,    34,    35,    36,    37,    38,    39,    40,    41,    42,
      43,    44,    45,    46,    47,    48,    49,    50,    51,    52,
      53,    54,    55,    56,    57,    58,    59,    60,    61,    62,
      63,    64,    65,     1,     1,     3,     4,     5,     6,     7,
       8,     9,    10,    11,    12,    13,    14,    15,    16,    17,
      18,    19,    20,    21,     1,    23,     1,    24,     5,    26,
       5,    29,    30,    31,    32,    33,    34,    35,    36,    37,
      38,    39,    40,    41,    42,    43,    44,    45,    46,    47,
      48,    49,    50,    51,    52,    53,    54,    55,    56,    57,
      58,    59,    60,    61,    62,    63,    64,    65,     1,     1,
       3,     4,     5,     6,     7,     8,     9,    10,    11,    12,
      13,    14,    15,    16,    17,    18,    19,    20,    21,     1,
      23,   151,    24,     5,    26,   155,    29,    30,    31,    32,
      33,    34,    35,    36,    37,    38,    39,    40,    41,    42,
      43,    44,    45,    46,    47,    48,    49,    50,    51,    52,
      53,    54,    55,    56,    57,    58,    59,    60,    61,    62,
      63,    64,    65,     1,     1,     3,     4,     5,     6,     7,
       8,     9,    10,    11,    12,    13,    14,    15,    16,    17,
      18,    19,    20,    21,    24,    23,    26,    24,    40,    26,
       5,    29,    30,    31,    32,    33,    34,    35,    36,    37,
      38,    39,    40,    41,    42,    43,    44,    45,    46,    47,
      48,    49,    50,    51,    52,    53,    54,    55,    56,    57,
      58,    59,    60,    61,    62,    63,    64,    65,     0,     1,
      10,     1,     4,     5,     4,    44,    16,     1,    47,     9,
      28,    11,    12,    13,    28,    15,    18,    17,   218,   219,
      30,    28,     1,    25,     3,   195,   196,    37,     7,    28,
      24,    41,    26,    35,    36,    14,    38,    39,    28,     1,
      19,    43,    21,    45,    46,   170,    48,     9,   173,     5,
      12,    30,   177,    15,   179,   195,   196,    29,    60,    61,
      62,    63,     3,     4,     5,     6,     7,     8,     9,    10,
      11,    12,    13,    14,    15,    16,    17,    18,    19,    20,
      21,    30,    23,    24,    27,    26,    27,    63,    29,    30,
      31,    32,    33,    34,    35,    36,    37,    38,    39,    40,
      41,    42,    43,    44,    45,    46,    47,    48,    49,    50,
      51,    52,    53,    54,    55,    56,    57,    58,    59,    60,
      61,    62,    63,    64,    65,     3,     4,     5,     6,     7,
       8,     9,    10,    11,    12,    13,    14,    15,    16,    17,
      18,    19,    20,    21,   311,    23,   194,   108,   315,   316,
     317,    29,    30,    31,    32,    33,    34,    35,    36,    37,
      38,    39,    40,    41,    42,    43,    44,    45,    46,    47,
      48,    49,    50,    51,    52,    53,    54,    55,    56,    57,
      58,    59,    60,    61,    62,    63,    64,    65,     1,   108,
     145,     4,     3,   243,    67,    23,     7,   118,    70,     1,
      28,    29,     4,    14,    -1,    18,    79,    80,    19,    24,
      21,    26,    27,    24,    -1,    26,    18,    -1,     9,    30,
      11,    12,    35,    36,    -1,    38,    39,    -1,    -1,    -1,
      43,    -1,    45,    46,    -1,    48,    38,    39,    -1,    -1,
      -1,    43,    -1,    45,    46,    -1,    48,    60,    61,     1,
      -1,     3,    -1,     5,     6,     7,     8,    -1,    60,    61,
      82,    83,    14,    -1,    86,    87,    18,    19,    20,    21,
       1,    -1,     3,    -1,     5,     6,     7,     8,    -1,    -1,
     102,   103,    -1,    14,    -1,    -1,    -1,    18,    19,    20,
      21,    96,    -1,    98,    99,   100,   101,    -1,    50,    51,
      52,    53,    54,    55,    56,    57,    58,    59,    -1,    -1,
     115,   116,    12,    13,    -1,    15,    -1,    17,    -1,    50,
      51,    52,    53,    54,    55,    56,    57,    58,    59,     1,
      -1,     3,    -1,     5,     6,     7,     8,    -1,    -1,    -1,
      -1,    -1,    14,    -1,    -1,    -1,    18,    19,    20,    21,
       1,    -1,     3,    -1,     5,     6,     7,     8,    -1,    -1,
      -1,    -1,    -1,    14,    -1,    -1,    -1,    18,    19,    20,
      21,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    50,    51,
      52,    53,    54,    55,    56,    57,    58,    59,     9,    -1,
      11,    12,    13,    -1,    15,    -1,    17,    -1,    -1,    50,
      51,    52,    53,    54,    55,    56,    57,    58,    59,     1,
      -1,     3,    -1,     5,     6,     7,     8,    -1,    -1,    -1,
      -1,    -1,    14,    -1,    -1,    -1,    18,    19,    20,    21,
       1,    -1,     3,    -1,     5,     6,     7,     8,    -1,    -1,
      -1,    -1,    -1,    14,    -1,    -1,    -1,    18,    19,    20,
      21,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    50,    51,
      52,    53,    54,    55,    56,    57,    58,    59,    -1,    -1,
      -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    50,
      51,    52,    53,    54,    55,    56,    57,    58,    59,     1,
      -1,     3,    -1,     5,     6,     7,     8,    -1,    -1,    -1,
      -1,    -1,    14,    -1,    -1,    -1,    18,    19,    20,    21,
       3,    -1,     5,     6,     7,     8,    -1,    -1,    -1,    -1,
      -1,    14,    -1,    -1,    -1,    18,    19,    20,    21,    -1,
      -1,    24,    -1,    26,    -1,    -1,    -1,    -1,    50,    51,
      52,    53,    54,    55,    56,    57,    58,    59,    -1,    -1,
      -1,    -1,    -1,    -1,    -1,    -1,    -1,    50,    51,    52,
      53,    54,    55,    56,    57,    58,    59,     3,    -1,     5,
       6,     7,     8,    -1,    -1,    -1,    -1,    -1,    14,    -1,
      -1,    -1,    18,    19,    20,    21,    -1,    -1,    -1,    -1,
      -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,
      -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,
      -1,    -1,    -1,    -1,    50,    51,    52,    53,    54,    55,
      56,    57,    58,    59
};

/* YYSTOS[STATE-NUM] -- The (internal number of the) accessing
   symbol of state STATE-NUM.  */
static const yytype_uint8 yystos[] =
{
       0,     1,     3,     4,     5,     6,     7,     8,     9,    10,
      11,    12,    13,    14,    15,    16,    17,    18,    19,    20,
      21,    23,    29,    30,    31,    32,    33,    34,    35,    36,
      37,    38,    39,    40,    41,    42,    43,    44,    45,    46,
      47,    48,    49,    50,    51,    52,    53,    54,    55,    56,
      57,    58,    59,    60,    61,    62,    63,    64,    65,    94,
     108,   111,   112,   113,     1,     5,    71,    72,    62,    67,
      68,     0,    24,    26,    27,    79,   100,   110,   112,     1,
      72,    23,     1,    23,    67,    71,     1,    22,    63,   103,
     105,   103,    67,     1,    67,     1,    74,   111,    74,    74,
      74,    74,     1,    22,     5,    25,    80,    88,   104,    79,
      79,    79,    79,     1,    79,    74,    74,     1,    22,     1,
      31,    32,    33,    80,    88,    79,     1,    79,     1,     4,
      18,    35,    36,    38,    39,    43,    45,    46,    48,    60,
      61,    77,    83,    84,    85,    86,    93,    99,   101,   102,
     106,   107,   109,   116,     1,   107,    28,     1,     5,    73,
       1,     5,    28,    73,     1,     5,    44,    47,    87,     1,
       3,     5,     6,     7,     8,    14,    18,    19,    20,    21,
      50,    51,    52,    53,    54,    55,    56,    57,    58,    59,
      70,    78,    98,   115,   117,     1,     4,     9,    11,    12,
      13,    15,    17,    90,    91,    96,    97,   114,     1,    19,
       1,    19,     1,   111,     1,   117,     1,    19,     1,    23,
       1,    62,    63,     1,    87,     1,    34,    40,   106,    28,
      23,    28,    29,    28,    28,    28,    28,    28,    10,    16,
      30,    37,    41,    42,    75,    76,    81,    82,    89,    92,
      95,   114,   114,   114,   114,     1,   117,     1,   117,     1,
      76,    78,    81,   110,     1,    78,    81,    12,    90,    90,
      96,    12,    96,     1,   110,    34,     1,    34,   110,     1,
     110,   110,     1,   110,    40,     1,    40,   116,   116,   110,
     110,   110,   110,     1,   110,     1,   110,    73,     5,     1,
       9,    12,    15,    69,     9,    15,     9,    15,    92,     1,
      49,   117,    29,     1,    29,   117,     1,   117,   110,   117,
     110,   117,     1,   110,     1,   117,   110,   110,     1,   110,
     110,   110,     1,   110,    30,     1,    30,   100,   117,   117,
       1,   117,   100,   100,   100,   110,   110,   110,     1,   110
};

#define yyerrok		(yyerrstatus = 0)
#define yyclearin	(yychar = YYEMPTY)
#define YYEMPTY		(-2)
#define YYEOF		0

#define YYACCEPT	goto yyacceptlab
#define YYABORT		goto yyabortlab
#define YYERROR		goto yyerrorlab


/* Like YYERROR except do call yyerror.  This remains here temporarily
   to ease the transition to the new meaning of YYERROR, for GCC.
   Once GCC version 2 has supplanted version 1, this can go.  However,
   YYFAIL appears to be in use.  Nevertheless, it is formally deprecated
   in Bison 2.4.2's NEWS entry, where a plan to phase it out is
   discussed.  */

#define YYFAIL		goto yyerrlab
#if defined YYFAIL
  /* This is here to suppress warnings from the GCC cpp's
     -Wunused-macros.  Normally we don't worry about that warning, but
     some users do, and we want to make it easy for users to remove
     YYFAIL uses, which will produce warnings from Bison 2.5.  */
#endif

#define YYRECOVERING()  (!!yyerrstatus)

#define YYBACKUP(Token, Value)					\
do								\
  if (yychar == YYEMPTY && yylen == 1)				\
    {								\
      yychar = (Token);						\
      yylval = (Value);						\
      yytoken = YYTRANSLATE (yychar);				\
      YYPOPSTACK (1);						\
      goto yybackup;						\
    }								\
  else								\
    {								\
      yyerror (YY_("syntax error: cannot back up")); \
      YYERROR;							\
    }								\
while (YYID (0))


#define YYTERROR	1
#define YYERRCODE	256


/* YYLLOC_DEFAULT -- Set CURRENT to span from RHS[1] to RHS[N].
   If N is 0, then set CURRENT to the empty location which ends
   the previous symbol: RHS[0] (always defined).  */

#define YYRHSLOC(Rhs, K) ((Rhs)[K])
#ifndef YYLLOC_DEFAULT
# define YYLLOC_DEFAULT(Current, Rhs, N)				\
    do									\
      if (YYID (N))                                                    \
	{								\
	  (Current).first_line   = YYRHSLOC (Rhs, 1).first_line;	\
	  (Current).first_column = YYRHSLOC (Rhs, 1).first_column;	\
	  (Current).last_line    = YYRHSLOC (Rhs, N).last_line;		\
	  (Current).last_column  = YYRHSLOC (Rhs, N).last_column;	\
	}								\
      else								\
	{								\
	  (Current).first_line   = (Current).last_line   =		\
	    YYRHSLOC (Rhs, 0).last_line;				\
	  (Current).first_column = (Current).last_column =		\
	    YYRHSLOC (Rhs, 0).last_column;				\
	}								\
    while (YYID (0))
#endif


/* YY_LOCATION_PRINT -- Print the location on the stream.
   This macro was not mandated originally: define only if we know
   we won't break user code: when these are the locations we know.  */

#ifndef YY_LOCATION_PRINT
# if defined YYLTYPE_IS_TRIVIAL && YYLTYPE_IS_TRIVIAL
#  define YY_LOCATION_PRINT(File, Loc)			\
     fprintf (File, "%d.%d-%d.%d",			\
	      (Loc).first_line, (Loc).first_column,	\
	      (Loc).last_line,  (Loc).last_column)
# else
#  define YY_LOCATION_PRINT(File, Loc) ((void) 0)
# endif
#endif


/* YYLEX -- calling `yylex' with the right arguments.  */

#ifdef YYLEX_PARAM
# define YYLEX yylex (YYLEX_PARAM)
#else
# define YYLEX yylex ()
#endif

/* Enable debugging if requested.  */
#if YYDEBUG

# ifndef YYFPRINTF
#  include <stdio.h> /* INFRINGES ON USER NAME SPACE */
#  define YYFPRINTF fprintf
# endif

# define YYDPRINTF(Args)			\
do {						\
  if (yydebug)					\
    YYFPRINTF Args;				\
} while (YYID (0))

# define YY_SYMBOL_PRINT(Title, Type, Value, Location)			  \
do {									  \
  if (yydebug)								  \
    {									  \
      YYFPRINTF (stderr, "%s ", Title);					  \
      yy_symbol_print (stderr,						  \
		  Type, Value); \
      YYFPRINTF (stderr, "\n");						  \
    }									  \
} while (YYID (0))


/*--------------------------------.
| Print this symbol on YYOUTPUT.  |
`--------------------------------*/

/*ARGSUSED*/
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static void
yy_symbol_value_print (FILE *yyoutput, int yytype, YYSTYPE const * const yyvaluep)
#else
static void
yy_symbol_value_print (yyoutput, yytype, yyvaluep)
    FILE *yyoutput;
    int yytype;
    YYSTYPE const * const yyvaluep;
#endif
{
  if (!yyvaluep)
    return;
# ifdef YYPRINT
  if (yytype < YYNTOKENS)
    YYPRINT (yyoutput, yytoknum[yytype], *yyvaluep);
# else
  YYUSE (yyoutput);
# endif
  switch (yytype)
    {
      default:
	break;
    }
}


/*--------------------------------.
| Print this symbol on YYOUTPUT.  |
`--------------------------------*/

#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static void
yy_symbol_print (FILE *yyoutput, int yytype, YYSTYPE const * const yyvaluep)
#else
static void
yy_symbol_print (yyoutput, yytype, yyvaluep)
    FILE *yyoutput;
    int yytype;
    YYSTYPE const * const yyvaluep;
#endif
{
  if (yytype < YYNTOKENS)
    YYFPRINTF (yyoutput, "token %s (", yytname[yytype]);
  else
    YYFPRINTF (yyoutput, "nterm %s (", yytname[yytype]);

  yy_symbol_value_print (yyoutput, yytype, yyvaluep);
  YYFPRINTF (yyoutput, ")");
}

/*------------------------------------------------------------------.
| yy_stack_print -- Print the state stack from its BOTTOM up to its |
| TOP (included).                                                   |
`------------------------------------------------------------------*/

#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static void
yy_stack_print (yytype_int16 *yybottom, yytype_int16 *yytop)
#else
static void
yy_stack_print (yybottom, yytop)
    yytype_int16 *yybottom;
    yytype_int16 *yytop;
#endif
{
  YYFPRINTF (stderr, "Stack now");
  for (; yybottom <= yytop; yybottom++)
    {
      int yybot = *yybottom;
      YYFPRINTF (stderr, " %d", yybot);
    }
  YYFPRINTF (stderr, "\n");
}

# define YY_STACK_PRINT(Bottom, Top)				\
do {								\
  if (yydebug)							\
    yy_stack_print ((Bottom), (Top));				\
} while (YYID (0))


/*------------------------------------------------.
| Report that the YYRULE is going to be reduced.  |
`------------------------------------------------*/

#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static void
yy_reduce_print (YYSTYPE *yyvsp, int yyrule)
#else
static void
yy_reduce_print (yyvsp, yyrule)
    YYSTYPE *yyvsp;
    int yyrule;
#endif
{
  int yynrhs = yyr2[yyrule];
  int yyi;
  unsigned long int yylno = yyrline[yyrule];
  YYFPRINTF (stderr, "Reducing stack by rule %d (line %lu):\n",
	     yyrule - 1, yylno);
  /* The symbols being reduced.  */
  for (yyi = 0; yyi < yynrhs; yyi++)
    {
      YYFPRINTF (stderr, "   $%d = ", yyi + 1);
      yy_symbol_print (stderr, yyrhs[yyprhs[yyrule] + yyi],
		       &(yyvsp[(yyi + 1) - (yynrhs)])
		       		       );
      YYFPRINTF (stderr, "\n");
    }
}

# define YY_REDUCE_PRINT(Rule)		\
do {					\
  if (yydebug)				\
    yy_reduce_print (yyvsp, Rule); \
} while (YYID (0))

/* Nonzero means print parse trace.  It is left uninitialized so that
   multiple parsers can coexist.  */
int yydebug;
#else /* !YYDEBUG */
# define YYDPRINTF(Args)
# define YY_SYMBOL_PRINT(Title, Type, Value, Location)
# define YY_STACK_PRINT(Bottom, Top)
# define YY_REDUCE_PRINT(Rule)
#endif /* !YYDEBUG */


/* YYINITDEPTH -- initial size of the parser's stacks.  */
#ifndef	YYINITDEPTH
# define YYINITDEPTH 200
#endif

/* YYMAXDEPTH -- maximum size the stacks can grow to (effective only
   if the built-in stack extension method is used).

   Do not make this value too large; the results are undefined if
   YYSTACK_ALLOC_MAXIMUM < YYSTACK_BYTES (YYMAXDEPTH)
   evaluated with infinite-precision integer arithmetic.  */

#ifndef YYMAXDEPTH
# define YYMAXDEPTH 10000
#endif



#if YYERROR_VERBOSE

# ifndef yystrlen
#  if defined __GLIBC__ && defined _STRING_H
#   define yystrlen strlen
#  else
/* Return the length of YYSTR.  */
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static YYSIZE_T
yystrlen (const char *yystr)
#else
static YYSIZE_T
yystrlen (yystr)
    const char *yystr;
#endif
{
  YYSIZE_T yylen;
  for (yylen = 0; yystr[yylen]; yylen++)
    continue;
  return yylen;
}
#  endif
# endif

# ifndef yystpcpy
#  if defined __GLIBC__ && defined _STRING_H && defined _GNU_SOURCE
#   define yystpcpy stpcpy
#  else
/* Copy YYSRC to YYDEST, returning the address of the terminating '\0' in
   YYDEST.  */
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static char *
yystpcpy (char *yydest, const char *yysrc)
#else
static char *
yystpcpy (yydest, yysrc)
    char *yydest;
    const char *yysrc;
#endif
{
  char *yyd = yydest;
  const char *yys = yysrc;

  while ((*yyd++ = *yys++) != '\0')
    continue;

  return yyd - 1;
}
#  endif
# endif

# ifndef yytnamerr
/* Copy to YYRES the contents of YYSTR after stripping away unnecessary
   quotes and backslashes, so that it's suitable for yyerror.  The
   heuristic is that double-quoting is unnecessary unless the string
   contains an apostrophe, a comma, or backslash (other than
   backslash-backslash).  YYSTR is taken from yytname.  If YYRES is
   null, do not copy; instead, return the length of what the result
   would have been.  */
static YYSIZE_T
yytnamerr (char *yyres, const char *yystr)
{
  if (*yystr == '"')
    {
      YYSIZE_T yyn = 0;
      char const *yyp = yystr;

      for (;;)
	switch (*++yyp)
	  {
	  case '\'':
	  case ',':
	    goto do_not_strip_quotes;

	  case '\\':
	    if (*++yyp != '\\')
	      goto do_not_strip_quotes;
	    /* Fall through.  */
	  default:
	    if (yyres)
	      yyres[yyn] = *yyp;
	    yyn++;
	    break;

	  case '"':
	    if (yyres)
	      yyres[yyn] = '\0';
	    return yyn;
	  }
    do_not_strip_quotes: ;
    }

  if (! yyres)
    return yystrlen (yystr);

  return yystpcpy (yyres, yystr) - yyres;
}
# endif

/* Copy into YYRESULT an error message about the unexpected token
   YYCHAR while in state YYSTATE.  Return the number of bytes copied,
   including the terminating null byte.  If YYRESULT is null, do not
   copy anything; just return the number of bytes that would be
   copied.  As a special case, return 0 if an ordinary "syntax error"
   message will do.  Return YYSIZE_MAXIMUM if overflow occurs during
   size calculation.  */
static YYSIZE_T
yysyntax_error (char *yyresult, int yystate, int yychar)
{
  int yyn = yypact[yystate];

  if (! (YYPACT_NINF < yyn && yyn <= YYLAST))
    return 0;
  else
    {
      int yytype = YYTRANSLATE (yychar);
      YYSIZE_T yysize0 = yytnamerr (0, yytname[yytype]);
      YYSIZE_T yysize = yysize0;
      YYSIZE_T yysize1;
      int yysize_overflow = 0;
      enum { YYERROR_VERBOSE_ARGS_MAXIMUM = 5 };
      char const *yyarg[YYERROR_VERBOSE_ARGS_MAXIMUM];
      int yyx;

# if 0
      /* This is so xgettext sees the translatable formats that are
	 constructed on the fly.  */
      YY_("syntax error, unexpected %s");
      YY_("syntax error, unexpected %s, expecting %s");
      YY_("syntax error, unexpected %s, expecting %s or %s");
      YY_("syntax error, unexpected %s, expecting %s or %s or %s");
      YY_("syntax error, unexpected %s, expecting %s or %s or %s or %s");
# endif
      char *yyfmt;
      char const *yyf;
      static char const yyunexpected[] = "syntax error, unexpected %s";
      static char const yyexpecting[] = ", expecting %s";
      static char const yyor[] = " or %s";
      char yyformat[sizeof yyunexpected
		    + sizeof yyexpecting - 1
		    + ((YYERROR_VERBOSE_ARGS_MAXIMUM - 2)
		       * (sizeof yyor - 1))];
      char const *yyprefix = yyexpecting;

      /* Start YYX at -YYN if negative to avoid negative indexes in
	 YYCHECK.  */
      int yyxbegin = yyn < 0 ? -yyn : 0;

      /* Stay within bounds of both yycheck and yytname.  */
      int yychecklim = YYLAST - yyn + 1;
      int yyxend = yychecklim < YYNTOKENS ? yychecklim : YYNTOKENS;
      int yycount = 1;

      yyarg[0] = yytname[yytype];
      yyfmt = yystpcpy (yyformat, yyunexpected);

      for (yyx = yyxbegin; yyx < yyxend; ++yyx)
	if (yycheck[yyx + yyn] == yyx && yyx != YYTERROR)
	  {
	    if (yycount == YYERROR_VERBOSE_ARGS_MAXIMUM)
	      {
		yycount = 1;
		yysize = yysize0;
		yyformat[sizeof yyunexpected - 1] = '\0';
		break;
	      }
	    yyarg[yycount++] = yytname[yyx];
	    yysize1 = yysize + yytnamerr (0, yytname[yyx]);
	    yysize_overflow |= (yysize1 < yysize);
	    yysize = yysize1;
	    yyfmt = yystpcpy (yyfmt, yyprefix);
	    yyprefix = yyor;
	  }

      yyf = YY_(yyformat);
      yysize1 = yysize + yystrlen (yyf);
      yysize_overflow |= (yysize1 < yysize);
      yysize = yysize1;

      if (yysize_overflow)
	return YYSIZE_MAXIMUM;

      if (yyresult)
	{
	  /* Avoid sprintf, as that infringes on the user's name space.
	     Don't have undefined behavior even if the translation
	     produced a string with the wrong number of "%s"s.  */
	  char *yyp = yyresult;
	  int yyi = 0;
	  while ((*yyp = *yyf) != '\0')
	    {
	      if (*yyp == '%' && yyf[1] == 's' && yyi < yycount)
		{
		  yyp += yytnamerr (yyp, yyarg[yyi++]);
		  yyf += 2;
		}
	      else
		{
		  yyp++;
		  yyf++;
		}
	    }
	}
      return yysize;
    }
}
#endif /* YYERROR_VERBOSE */


/*-----------------------------------------------.
| Release the memory associated to this symbol.  |
`-----------------------------------------------*/

/*ARGSUSED*/
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
static void
yydestruct (const char *yymsg, int yytype, YYSTYPE *yyvaluep)
#else
static void
yydestruct (yymsg, yytype, yyvaluep)
    const char *yymsg;
    int yytype;
    YYSTYPE *yyvaluep;
#endif
{
  YYUSE (yyvaluep);

  if (!yymsg)
    yymsg = "Deleting";
  YY_SYMBOL_PRINT (yymsg, yytype, yyvaluep, yylocationp);

  switch (yytype)
    {

      default:
	break;
    }
}

/* Prevent warnings from -Wmissing-prototypes.  */
#ifdef YYPARSE_PARAM
#if defined __STDC__ || defined __cplusplus
int yyparse (void *YYPARSE_PARAM);
#else
int yyparse ();
#endif
#else /* ! YYPARSE_PARAM */
#if defined __STDC__ || defined __cplusplus
int yyparse (void);
#else
int yyparse ();
#endif
#endif /* ! YYPARSE_PARAM */


/* The lookahead symbol.  */
int yychar;

/* The semantic value of the lookahead symbol.  */
YYSTYPE yylval;

/* Number of syntax errors so far.  */
int yynerrs;



/*-------------------------.
| yyparse or yypush_parse.  |
`-------------------------*/

#ifdef YYPARSE_PARAM
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
int
yyparse (void *YYPARSE_PARAM)
#else
int
yyparse (YYPARSE_PARAM)
    void *YYPARSE_PARAM;
#endif
#else /* ! YYPARSE_PARAM */
#if (defined __STDC__ || defined __C99__FUNC__ \
     || defined __cplusplus || defined _MSC_VER)
int
yyparse (void)
#else
int
yyparse ()

#endif
#endif
{


    int yystate;
    /* Number of tokens to shift before error messages enabled.  */
    int yyerrstatus;

    /* The stacks and their tools:
       `yyss': related to states.
       `yyvs': related to semantic values.

       Refer to the stacks thru separate pointers, to allow yyoverflow
       to reallocate them elsewhere.  */

    /* The state stack.  */
    yytype_int16 yyssa[YYINITDEPTH];
    yytype_int16 *yyss;
    yytype_int16 *yyssp;

    /* The semantic value stack.  */
    YYSTYPE yyvsa[YYINITDEPTH];
    YYSTYPE *yyvs;
    YYSTYPE *yyvsp;

    YYSIZE_T yystacksize;

  int yyn;
  int yyresult;
  /* Lookahead token as an internal (translated) token number.  */
  int yytoken;
  /* The variables used to return semantic value and location from the
     action routines.  */
  YYSTYPE yyval;

#if YYERROR_VERBOSE
  /* Buffer for error messages, and its allocated size.  */
  char yymsgbuf[128];
  char *yymsg = yymsgbuf;
  YYSIZE_T yymsg_alloc = sizeof yymsgbuf;
#endif

#define YYPOPSTACK(N)   (yyvsp -= (N), yyssp -= (N))

  /* The number of symbols on the RHS of the reduced rule.
     Keep to zero when no symbol should be popped.  */
  int yylen = 0;

  yytoken = 0;
  yyss = yyssa;
  yyvs = yyvsa;
  yystacksize = YYINITDEPTH;

  YYDPRINTF ((stderr, "Starting parse\n"));

  yystate = 0;
  yyerrstatus = 0;
  yynerrs = 0;
  yychar = YYEMPTY; /* Cause a token to be read.  */

  /* Initialize stack pointers.
     Waste one element of value and location stack
     so that they stay on the same level as the state stack.
     The wasted elements are never initialized.  */
  yyssp = yyss;
  yyvsp = yyvs;

  goto yysetstate;

/*------------------------------------------------------------.
| yynewstate -- Push a new state, which is found in yystate.  |
`------------------------------------------------------------*/
 yynewstate:
  /* In all cases, when you get here, the value and location stacks
     have just been pushed.  So pushing a state here evens the stacks.  */
  yyssp++;

 yysetstate:
  *yyssp = yystate;

  if (yyss + yystacksize - 1 <= yyssp)
    {
      /* Get the current used size of the three stacks, in elements.  */
      YYSIZE_T yysize = yyssp - yyss + 1;

#ifdef yyoverflow
      {
	/* Give user a chance to reallocate the stack.  Use copies of
	   these so that the &'s don't force the real ones into
	   memory.  */
	YYSTYPE *yyvs1 = yyvs;
	yytype_int16 *yyss1 = yyss;

	/* Each stack pointer address is followed by the size of the
	   data in use in that stack, in bytes.  This used to be a
	   conditional around just the two extra args, but that might
	   be undefined if yyoverflow is a macro.  */
	yyoverflow (YY_("memory exhausted"),
		    &yyss1, yysize * sizeof (*yyssp),
		    &yyvs1, yysize * sizeof (*yyvsp),
		    &yystacksize);

	yyss = yyss1;
	yyvs = yyvs1;
      }
#else /* no yyoverflow */
# ifndef YYSTACK_RELOCATE
      goto yyexhaustedlab;
# else
      /* Extend the stack our own way.  */
      if (YYMAXDEPTH <= yystacksize)
	goto yyexhaustedlab;
      yystacksize *= 2;
      if (YYMAXDEPTH < yystacksize)
	yystacksize = YYMAXDEPTH;

      {
	yytype_int16 *yyss1 = yyss;
	union yyalloc *yyptr =
	  (union yyalloc *) YYSTACK_ALLOC (YYSTACK_BYTES (yystacksize));
	if (! yyptr)
	  goto yyexhaustedlab;
	YYSTACK_RELOCATE (yyss_alloc, yyss);
	YYSTACK_RELOCATE (yyvs_alloc, yyvs);
#  undef YYSTACK_RELOCATE
	if (yyss1 != yyssa)
	  YYSTACK_FREE (yyss1);
      }
# endif
#endif /* no yyoverflow */

      yyssp = yyss + yysize - 1;
      yyvsp = yyvs + yysize - 1;

      YYDPRINTF ((stderr, "Stack size increased to %lu\n",
		  (unsigned long int) yystacksize));

      if (yyss + yystacksize - 1 <= yyssp)
	YYABORT;
    }

  YYDPRINTF ((stderr, "Entering state %d\n", yystate));

  if (yystate == YYFINAL)
    YYACCEPT;

  goto yybackup;

/*-----------.
| yybackup.  |
`-----------*/
yybackup:

  /* Do appropriate processing given the current state.  Read a
     lookahead token if we need one and don't already have one.  */

  /* First try to decide what to do without reference to lookahead token.  */
  yyn = yypact[yystate];
  if (yyn == YYPACT_NINF)
    goto yydefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* YYCHAR is either YYEMPTY or YYEOF or a valid lookahead symbol.  */
  if (yychar == YYEMPTY)
    {
      YYDPRINTF ((stderr, "Reading a token: "));
      yychar = YYLEX;
    }

  if (yychar <= YYEOF)
    {
      yychar = yytoken = YYEOF;
      YYDPRINTF ((stderr, "Now at end of input.\n"));
    }
  else
    {
      yytoken = YYTRANSLATE (yychar);
      YY_SYMBOL_PRINT ("Next token is", yytoken, &yylval, &yylloc);
    }

  /* If the proper action on seeing token YYTOKEN is to reduce or to
     detect an error, take that action.  */
  yyn += yytoken;
  if (yyn < 0 || YYLAST < yyn || yycheck[yyn] != yytoken)
    goto yydefault;
  yyn = yytable[yyn];
  if (yyn <= 0)
    {
      if (yyn == 0 || yyn == YYTABLE_NINF)
	goto yyerrlab;
      yyn = -yyn;
      goto yyreduce;
    }

  /* Count tokens shifted since error; after three, turn off error
     status.  */
  if (yyerrstatus)
    yyerrstatus--;

  /* Shift the lookahead token.  */
  YY_SYMBOL_PRINT ("Shifting", yytoken, &yylval, &yylloc);

  /* Discard the shifted token.  */
  yychar = YYEMPTY;

  yystate = yyn;
  *++yyvsp = yylval;

  goto yynewstate;


/*-----------------------------------------------------------.
| yydefault -- do the default action for the current state.  |
`-----------------------------------------------------------*/
yydefault:
  yyn = yydefact[yystate];
  if (yyn == 0)
    goto yyerrlab;
  goto yyreduce;


/*-----------------------------.
| yyreduce -- Do a reduction.  |
`-----------------------------*/
yyreduce:
  /* yyn is the number of a rule to reduce with.  */
  yylen = yyr2[yyn];

  /* If YYLEN is nonzero, implement the default value of the action:
     `$$ = $1'.

     Otherwise, the following line sets YYVAL to garbage.
     This behavior is undocumented and Bison
     users should not rely upon it.  Assigning to YYVAL
     unconditionally makes the parser a bit smaller, and it avoids a
     GCC warning that YYVAL may be used uninitialized.  */
  yyval = yyvsp[1-yylen];


  YY_REDUCE_PRINT (yyn);
  switch (yyn)
    {
        case 2:

/* Line 1464 of yacc.c  */
#line 185 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));
;}
    break;

  case 3:

/* Line 1464 of yacc.c  */
#line 188 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));
;}
    break;

  case 4:

/* Line 1464 of yacc.c  */
#line 193 "grammar.y"
    {
  free(current_act);
  current_act = newstr(str2varname((yyvsp[(1) - (4)].str)));
  (yyval.str) = cat4(newstr("\n"), strpad(cat2(newstr(current_act), newstr(":")), COMMENT_COLUMN, ' '), (yyvsp[(3) - (4)].str), newstr("\n"));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 5:

/* Line 1464 of yacc.c  */
#line 200 "grammar.y"
    {
  report_warning("period or exclamation mark");
  free(current_act);
  current_act = newstr(str2varname((yyvsp[(1) - (4)].str)));
  (yyval.str) = cat4(newstr("\n"), strpad(cat2(newstr(current_act), newstr(":")), COMMENT_COLUMN, ' '), (yyvsp[(3) - (4)].str), newstr("\n"));
  free((yyvsp[(2) - (4)].str));
;}
    break;

  case 6:

/* Line 1464 of yacc.c  */
#line 207 "grammar.y"
    {
  report_warning("colon");
  free(current_act);
  current_act = newstr(str2varname((yyvsp[(1) - (4)].str)));
  (yyval.str) = cat4(newstr("\n"), strpad(cat2(newstr(current_act), newstr(":")), COMMENT_COLUMN, ' '), (yyvsp[(3) - (4)].str), newstr("\n"));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 7:

/* Line 1464 of yacc.c  */
#line 216 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 8:

/* Line 1464 of yacc.c  */
#line 219 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 9:

/* Line 1464 of yacc.c  */
#line 222 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 10:

/* Line 1464 of yacc.c  */
#line 227 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(3*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_sub("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(", ");
  (yyval.stringlist).list[2] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 11:

/* Line 1464 of yacc.c  */
#line 234 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(3*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_mul("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(", ");
  (yyval.stringlist).list[2] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 12:

/* Line 1464 of yacc.c  */
#line 241 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(3*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_div("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(", ");
  (yyval.stringlist).list[2] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 13:

/* Line 1464 of yacc.c  */
#line 248 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(3*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_mod("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(", ");
  (yyval.stringlist).list[2] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 14:

/* Line 1464 of yacc.c  */
#line 255 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(3*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_add("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(", ");
  (yyval.stringlist).list[2] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 15:

/* Line 1464 of yacc.c  */
#line 264 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(strpad(cat3(newstr("CHARACTER *"), str2varname(newstr((yyvsp[(1) - (4)].str))),
				newstr(";")), COMMENT_COLUMN, ' '),
		    (yyvsp[(3) - (4)].str), newstr("\n"));
  (yyval.stringlist).list[1] = cat4(str2varname(newstr((yyvsp[(1) - (4)].str))), newstr(" = initialize_character(\""),
		    (yyvsp[(1) - (4)].str), newstr("\");\n"));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 16:

/* Line 1464 of yacc.c  */
#line 274 "grammar.y"
    {
  report_error("character name");
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = newstr("");
  (yyval.stringlist).list[1] = newstr("");
;}
    break;

  case 17:

/* Line 1464 of yacc.c  */
#line 280 "grammar.y"
    {
  report_error("comma");
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = newstr("");
  (yyval.stringlist).list[1] = newstr("");
;}
    break;

  case 18:

/* Line 1464 of yacc.c  */
#line 288 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = (yyvsp[(1) - (1)].stringlist).list[0];
  (yyval.stringlist).list[1] = (yyvsp[(1) - (1)].stringlist).list[1];
  free((yyvsp[(1) - (1)].stringlist).list);
;}
    break;

  case 19:

/* Line 1464 of yacc.c  */
#line 294 "grammar.y"
    {
  (yyval.stringlist).list = (yyvsp[(1) - (2)].stringlist).list;
  (yyval.stringlist).list[0] = cat2((yyvsp[(1) - (2)].stringlist).list[0], (yyvsp[(2) - (2)].stringlist).list[0]);
  (yyval.stringlist).list[1] = cat2((yyvsp[(1) - (2)].stringlist).list[1], (yyvsp[(2) - (2)].stringlist).list[1]);
  free((yyvsp[(2) - (2)].stringlist).list);
;}
    break;

  case 20:

/* Line 1464 of yacc.c  */
#line 302 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = (yyvsp[(1) - (3)].str);
  (yyval.stringlist).list[1] = (yyvsp[(3) - (3)].str);
  (yyval.stringlist).num = 2;
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 21:

/* Line 1464 of yacc.c  */
#line 309 "grammar.y"
    {
  (yyval.stringlist).num = (yyvsp[(3) - (3)].stringlist).num + 1;
  (yyval.stringlist).list = realloc((yyvsp[(3) - (3)].stringlist).list, (yyval.stringlist).num*sizeof(char **));
  (yyval.stringlist).list[(yyval.stringlist).num - 1] = (yyvsp[(1) - (3)].str);
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 22:

/* Line 1464 of yacc.c  */
#line 317 "grammar.y"
    {
  (yyval.str) = cat3(newstr("/* "), (yyvsp[(1) - (1)].str), newstr(" */"));
;}
    break;

  case 23:

/* Line 1464 of yacc.c  */
#line 320 "grammar.y"
    {
  report_warning("comment");
  (yyval.str) = newstr("/* NO COMMENT FOUND */");
;}
    break;

  case 24:

/* Line 1464 of yacc.c  */
#line 326 "grammar.y"
    {
  (yyval.str) = newstr("comp1 < comp2");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 25:

/* Line 1464 of yacc.c  */
#line 330 "grammar.y"
    {
  (yyval.str) = newstr("comp1 > comp2");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 26:

/* Line 1464 of yacc.c  */
#line 336 "grammar.y"
    {
  (yyval.str) = cat2(newstr("!"), (yyvsp[(2) - (2)].str));
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 27:

/* Line 1464 of yacc.c  */
#line 340 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 28:

/* Line 1464 of yacc.c  */
#line 345 "grammar.y"
    {
  (yyval.str) = newstr("truth_flag");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 29:

/* Line 1464 of yacc.c  */
#line 349 "grammar.y"
    {
  (yyval.str) = newstr("!truth_flag");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 30:

/* Line 1464 of yacc.c  */
#line 355 "grammar.y"
    {
  (yyval.str) = (yyvsp[(2) - (2)].str);
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 31:

/* Line 1464 of yacc.c  */
#line 359 "grammar.y"
    {
  (yyval.str) = (yyvsp[(2) - (2)].str);
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 32:

/* Line 1464 of yacc.c  */
#line 363 "grammar.y"
    {
  (yyval.str) = (yyvsp[(2) - (2)].str);
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 33:

/* Line 1464 of yacc.c  */
#line 367 "grammar.y"
    {
  (yyval.str) = (yyvsp[(2) - (2)].str);
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 34:

/* Line 1464 of yacc.c  */
#line 371 "grammar.y"
    {
  (yyval.str) = newstr("0");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 35:

/* Line 1464 of yacc.c  */
#line 377 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 36:

/* Line 1464 of yacc.c  */
#line 380 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 37:

/* Line 1464 of yacc.c  */
#line 385 "grammar.y"
    {
  (yyval.str) = cat5(newstr("\nenter_scene("), int2str(yylineno), newstr(", "), str2varname((yyvsp[(3) - (4)].str)), newstr(");\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 38:

/* Line 1464 of yacc.c  */
#line 391 "grammar.y"
    {
  (yyval.str) = newstr("\n");
  for (i = 0; i < (yyvsp[(3) - (4)].stringlist).num; i++) {
    (yyval.str) = cat6((yyval.str), newstr("enter_scene("), int2str(yylineno), newstr(", "),
	      str2varname((yyvsp[(3) - (4)].stringlist).list[i]), newstr(");\n"));
  }
  free((yyvsp[(3) - (4)].stringlist).list);
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 39:

/* Line 1464 of yacc.c  */
#line 402 "grammar.y"
    {
  (yyval.str) = cat5(newstr("\nexit_scene("), int2str(yylineno), newstr(", "), str2varname((yyvsp[(3) - (4)].str)), newstr(");\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 40:

/* Line 1464 of yacc.c  */
#line 408 "grammar.y"
    {
  (yyval.str) = newstr("\n");
  for (i = 0; i < (yyvsp[(3) - (4)].stringlist).num; i++) {
    (yyval.str) = cat6((yyval.str), newstr("exit_scene("), int2str(yylineno), newstr(", "),
	      str2varname((yyvsp[(3) - (4)].stringlist).list[i]), newstr(");\n"));
  }
  free((yyvsp[(3) - (4)].stringlist).list);
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 41:

/* Line 1464 of yacc.c  */
#line 419 "grammar.y"
    {
  (yyval.str) = cat3(newstr("\nexit_scene_all("), int2str(yylineno), newstr(");\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 42:

/* Line 1464 of yacc.c  */
#line 425 "grammar.y"
    {
  report_error("character or character list");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 43:

/* Line 1464 of yacc.c  */
#line 432 "grammar.y"
    {
  report_error("character");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 44:

/* Line 1464 of yacc.c  */
#line 439 "grammar.y"
    {
  report_error("character list or nothing");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 45:

/* Line 1464 of yacc.c  */
#line 446 "grammar.y"
    {
  report_error("'enter', 'exit' or 'exeunt'");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 46:

/* Line 1464 of yacc.c  */
#line 454 "grammar.y"
    {
  (yyval.str) = newstr("comp1 == comp2");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 47:

/* Line 1464 of yacc.c  */
#line 460 "grammar.y"
    {
  report_error("adjective");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 48:

/* Line 1464 of yacc.c  */
#line 466 "grammar.y"
    {
  report_error("as");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 49:

/* Line 1464 of yacc.c  */
#line 474 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (2)].str);
  free((yyvsp[(2) - (2)].str));
;}
    break;

  case 50:

/* Line 1464 of yacc.c  */
#line 478 "grammar.y"
    {
  report_warning("'than'");
  (yyval.str) = (yyvsp[(1) - (2)].str);
;}
    break;

  case 51:

/* Line 1464 of yacc.c  */
#line 484 "grammar.y"
    {
  (yyval.str) = cat3(newstr("int_output("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 52:

/* Line 1464 of yacc.c  */
#line 490 "grammar.y"
    {
  (yyval.str) = cat3(newstr("char_output("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(3) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 53:

/* Line 1464 of yacc.c  */
#line 497 "grammar.y"
    {
  (yyval.str) = cat3(newstr("int_input("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(3) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 54:

/* Line 1464 of yacc.c  */
#line 504 "grammar.y"
    {
  (yyval.str) = cat3(newstr("char_input("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 55:

/* Line 1464 of yacc.c  */
#line 510 "grammar.y"
    {
  report_error("'mind' or 'heart'");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 56:

/* Line 1464 of yacc.c  */
#line 516 "grammar.y"
    {
  report_warning("possessive pronoun, second person");
  (yyval.str) = cat3(newstr("char_output("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(3) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 57:

/* Line 1464 of yacc.c  */
#line 523 "grammar.y"
    {
  report_warning("possessive pronoun, second person");
  (yyval.str) = cat3(newstr("int_input("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(3) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 58:

/* Line 1464 of yacc.c  */
#line 530 "grammar.y"
    {
  report_warning("'mind'");
  (yyval.str) = cat3(newstr("char_output("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 59:

/* Line 1464 of yacc.c  */
#line 537 "grammar.y"
    {
  report_warning("'heart'");
  (yyval.str) = cat3(newstr("int_input("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 60:

/* Line 1464 of yacc.c  */
#line 544 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat3(newstr("int_output("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 61:

/* Line 1464 of yacc.c  */
#line 550 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat3(newstr("char_output("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(3) - (4)].str));
;}
    break;

  case 62:

/* Line 1464 of yacc.c  */
#line 557 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat3(newstr("int_input("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(3) - (4)].str));
;}
    break;

  case 63:

/* Line 1464 of yacc.c  */
#line 564 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat3(newstr("char_input("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 64:

/* Line 1464 of yacc.c  */
#line 572 "grammar.y"
    {
  (yyval.str) = cat3(newstr("goto "), str2varname((yyvsp[(2) - (3)].str)), newstr(";\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 65:

/* Line 1464 of yacc.c  */
#line 577 "grammar.y"
    {
  (yyval.str) = cat5(newstr("goto "), newstr(current_act), newstr("_"),
	    str2varname((yyvsp[(2) - (3)].str)), newstr(";\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 66:

/* Line 1464 of yacc.c  */
#line 583 "grammar.y"
    {
  report_error("'act [roman number]' or 'scene [roman number]'");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 67:

/* Line 1464 of yacc.c  */
#line 591 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].str), newstr(" "), (yyvsp[(2) - (2)].str));
;}
    break;

  case 68:

/* Line 1464 of yacc.c  */
#line 594 "grammar.y"
    {
  report_warning("'let us', 'we must' or 'we shall'");
  (yyval.str) = newstr("");
  free((yyvsp[(2) - (2)].str));
;}
    break;

  case 69:

/* Line 1464 of yacc.c  */
#line 599 "grammar.y"
    {
  report_warning("'proceed to' or 'return to'");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 70:

/* Line 1464 of yacc.c  */
#line 606 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 71:

/* Line 1464 of yacc.c  */
#line 609 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 72:

/* Line 1464 of yacc.c  */
#line 612 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 73:

/* Line 1464 of yacc.c  */
#line 617 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 74:

/* Line 1464 of yacc.c  */
#line 620 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 75:

/* Line 1464 of yacc.c  */
#line 625 "grammar.y"
    {
  (yyval.str) = cat6(newstr("\nactivate_character("), int2str(yylineno), newstr(", "), str2varname((yyvsp[(1) - (3)].str)),
	    newstr(");\n"), (yyvsp[(3) - (3)].str));
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 76:

/* Line 1464 of yacc.c  */
#line 630 "grammar.y"
    {
  report_error("sentence list");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 77:

/* Line 1464 of yacc.c  */
#line 636 "grammar.y"
    {
  report_error("colon");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 78:

/* Line 1464 of yacc.c  */
#line 644 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 79:

/* Line 1464 of yacc.c  */
#line 647 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].str), newstr(" "), (yyvsp[(2) - (2)].str));
;}
    break;

  case 80:

/* Line 1464 of yacc.c  */
#line 650 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].str), newstr(" "), (yyvsp[(2) - (2)].str));
;}
    break;

  case 81:

/* Line 1464 of yacc.c  */
#line 655 "grammar.y"
    {
  (yyval.str) = newstr("(-1)");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 82:

/* Line 1464 of yacc.c  */
#line 659 "grammar.y"
    {
  (yyval.str) = cat2(newstr("2*"), (yyvsp[(2) - (2)].str));
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 83:

/* Line 1464 of yacc.c  */
#line 663 "grammar.y"
    {
  (yyval.str) = cat2(newstr("2*"), (yyvsp[(2) - (2)].str));
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 84:

/* Line 1464 of yacc.c  */
#line 669 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 85:

/* Line 1464 of yacc.c  */
#line 674 "grammar.y"
    {
  (yyval.str) = cat3(newstr("("), (yyvsp[(1) - (1)].str), newstr(")"));;
;}
    break;

  case 86:

/* Line 1464 of yacc.c  */
#line 677 "grammar.y"
    {
  (yyval.str) = cat3(newstr("("), (yyvsp[(1) - (1)].str), newstr(")"));;
;}
    break;

  case 87:

/* Line 1464 of yacc.c  */
#line 682 "grammar.y"
    {
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (2)].str));
  free((yyvsp[(2) - (2)].str));
;}
    break;

  case 88:

/* Line 1464 of yacc.c  */
#line 687 "grammar.y"
    {
  report_warning("possessive pronoun, second person");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 89:

/* Line 1464 of yacc.c  */
#line 694 "grammar.y"
    {
  (yyval.str) = cat6((yyvsp[(1) - (3)].str), newstr("\n"), (yyvsp[(2) - (3)].stringlist).list[0], 
	    newstr("\nint comp1, comp2;\n\nglobal_initialize();\n\n"),
	    (yyvsp[(2) - (3)].stringlist).list[1], (yyvsp[(3) - (3)].str));
  free((yyvsp[(2) - (3)].stringlist).list);
;}
    break;

  case 90:

/* Line 1464 of yacc.c  */
#line 700 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));  
;}
    break;

  case 91:

/* Line 1464 of yacc.c  */
#line 703 "grammar.y"
    {
  report_error("act");
  (yyval.str) = newstr("");
  free((yyvsp[(2) - (3)].stringlist).list[0]);
  free((yyvsp[(2) - (3)].stringlist).list[1]);
  free((yyvsp[(2) - (3)].stringlist).list);
;}
    break;

  case 92:

/* Line 1464 of yacc.c  */
#line 710 "grammar.y"
    {
  report_error("character declaration list");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 93:

/* Line 1464 of yacc.c  */
#line 716 "grammar.y"
    {
  report_warning("title");
  (yyval.str) = cat4((yyvsp[(2) - (3)].stringlist).list[0], newstr("\n"), (yyvsp[(2) - (3)].stringlist).list[1], (yyvsp[(3) - (3)].str));
  free((yyvsp[(2) - (3)].stringlist).list);
;}
    break;

  case 94:

/* Line 1464 of yacc.c  */
#line 723 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 95:

/* Line 1464 of yacc.c  */
#line 726 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].str), newstr(" "), (yyvsp[(2) - (2)].str));
;}
    break;

  case 96:

/* Line 1464 of yacc.c  */
#line 729 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].str), newstr(" "), (yyvsp[(2) - (2)].str));
;}
    break;

  case 97:

/* Line 1464 of yacc.c  */
#line 734 "grammar.y"
    {
  (yyval.str) = newstr("1");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 98:

/* Line 1464 of yacc.c  */
#line 738 "grammar.y"
    {
  (yyval.str) = cat2(newstr("2*"), (yyvsp[(2) - (2)].str));
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 99:

/* Line 1464 of yacc.c  */
#line 742 "grammar.y"
    {
  (yyval.str) = cat2(newstr("2*"), (yyvsp[(2) - (2)].str));
  free((yyvsp[(1) - (2)].str));
;}
    break;

  case 100:

/* Line 1464 of yacc.c  */
#line 748 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 101:

/* Line 1464 of yacc.c  */
#line 751 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 102:

/* Line 1464 of yacc.c  */
#line 756 "grammar.y"
    {
  (yyval.str) = newstr("first_person");
;}
    break;

  case 103:

/* Line 1464 of yacc.c  */
#line 759 "grammar.y"
    {
  (yyval.str) = newstr("first_person");
;}
    break;

  case 104:

/* Line 1464 of yacc.c  */
#line 762 "grammar.y"
    {
  (yyval.str) = newstr("second_person");
;}
    break;

  case 105:

/* Line 1464 of yacc.c  */
#line 765 "grammar.y"
    {
  (yyval.str) = newstr("second_person");
;}
    break;

  case 106:

/* Line 1464 of yacc.c  */
#line 770 "grammar.y"
    {
  (yyval.str) = cat9(newstr("comp1 = "), (yyvsp[(2) - (5)].str), newstr(";\n"),
	    newstr("comp2 = "), (yyvsp[(4) - (5)].str), newstr(";\n"),
	    newstr("truth_flag = "), (yyvsp[(3) - (5)].str), newstr(";\n"));
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 107:

/* Line 1464 of yacc.c  */
#line 777 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(3) - (5)].str));
  free((yyvsp[(4) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 108:

/* Line 1464 of yacc.c  */
#line 785 "grammar.y"
    {
  report_error("comparison");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(2) - (5)].str));
  free((yyvsp[(4) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 109:

/* Line 1464 of yacc.c  */
#line 793 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(2) - (5)].str));
  free((yyvsp[(3) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 110:

/* Line 1464 of yacc.c  */
#line 803 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 111:

/* Line 1464 of yacc.c  */
#line 808 "grammar.y"
    {
  (yyval.str) = cat3(newstr("pop("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 112:

/* Line 1464 of yacc.c  */
#line 814 "grammar.y"
    {
  report_warning("string");
  (yyval.str) = cat3(newstr("pop("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 113:

/* Line 1464 of yacc.c  */
#line 820 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat3(newstr("pop("), int2str(yylineno), newstr(", second_person);\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(2) - (3)].str));
;}
    break;

  case 114:

/* Line 1464 of yacc.c  */
#line 828 "grammar.y"
    {
  (yyval.str) = cat5(newstr("push("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(2) - (3)].str), newstr(");\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 115:

/* Line 1464 of yacc.c  */
#line 833 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 116:

/* Line 1464 of yacc.c  */
#line 839 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat5(newstr("push("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(2) - (3)].str), newstr(");\n"));
  free((yyvsp[(1) - (3)].str));
;}
    break;

  case 117:

/* Line 1464 of yacc.c  */
#line 846 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));
;}
    break;

  case 118:

/* Line 1464 of yacc.c  */
#line 851 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 119:

/* Line 1464 of yacc.c  */
#line 854 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 120:

/* Line 1464 of yacc.c  */
#line 857 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));
;}
    break;

  case 121:

/* Line 1464 of yacc.c  */
#line 860 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));
;}
    break;

  case 122:

/* Line 1464 of yacc.c  */
#line 865 "grammar.y"
    {
  free(current_scene);
  current_scene = cat3(newstr(current_act), newstr("_"), str2varname((yyvsp[(1) - (4)].str)));
  (yyval.str) = cat4(newstr("\n"), strpad(cat2(newstr(current_scene), newstr(":")), COMMENT_COLUMN, ' '), (yyvsp[(3) - (4)].str), newstr("\n"));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 123:

/* Line 1464 of yacc.c  */
#line 872 "grammar.y"
    {
  report_warning("period or exclamation mark");
  free(current_scene);
  current_scene = cat3(newstr(current_act), newstr("_"), str2varname((yyvsp[(1) - (4)].str)));
  (yyval.str) = cat4(newstr("\n"), strpad(cat2(newstr(current_scene), newstr(":")), COMMENT_COLUMN, ' '), (yyvsp[(3) - (4)].str), newstr("\n"));
  free((yyvsp[(2) - (4)].str));
;}
    break;

  case 124:

/* Line 1464 of yacc.c  */
#line 879 "grammar.y"
    {
  report_warning("colon");
  free(current_scene);
  current_scene = cat3(newstr(current_act), newstr("_"), str2varname((yyvsp[(1) - (4)].str)));
  (yyval.str) = cat4(newstr("\n"), strpad(cat2(newstr(current_scene), newstr(":")), COMMENT_COLUMN, ' '), (yyvsp[(3) - (4)].str), newstr("\n"));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 125:

/* Line 1464 of yacc.c  */
#line 888 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 126:

/* Line 1464 of yacc.c  */
#line 891 "grammar.y"
    {
  (yyval.str) = cat5(newstr("if ("), (yyvsp[(1) - (3)].str), newstr(") {\n"), strindent((yyvsp[(3) - (3)].str), INDENT), newstr("}\n"));
;}
    break;

  case 127:

/* Line 1464 of yacc.c  */
#line 894 "grammar.y"
    {
  report_warning("comma");
  (yyval.str) = cat5(newstr("if ("), (yyvsp[(1) - (3)].str), newstr(") {\n"), strindent((yyvsp[(3) - (3)].str), INDENT), newstr("}\n"));
;}
    break;

  case 128:

/* Line 1464 of yacc.c  */
#line 900 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 129:

/* Line 1464 of yacc.c  */
#line 903 "grammar.y"
    {
  (yyval.str) = cat2((yyvsp[(1) - (2)].str), (yyvsp[(2) - (2)].str));
;}
    break;

  case 130:

/* Line 1464 of yacc.c  */
#line 908 "grammar.y"
    {
  char *program;

  program = strindent(cat2((yyvsp[(1) - (1)].str), newstr("\nreturn 0;")), INDENT);
  if (num_errors == 0) {
    printf("/********************************************************************\n");
    printf(" *   This C program was generated by spl2c, the Shakespeare to C    *\n");
    printf(" *          converter by Jon Åslund and Karl Hasselström.           *\n");
    printf(" ********************************************************************/\n");
    printf("\n");
    printf("/* libspl definitions and function prototypes */\n");
    printf("#include <spl.h>\n");
    printf("\n");
    printf("int main(void)\n");
    printf("{\n");
    printf("%s\n", program);
    printf("}\n");
  }
  free(program);
;}
    break;

  case 131:

/* Line 1464 of yacc.c  */
#line 930 "grammar.y"
    {
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(3) - (4)].str), newstr(");\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 132:

/* Line 1464 of yacc.c  */
#line 936 "grammar.y"
    {
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(2) - (3)].str), newstr(");\n"));
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 133:

/* Line 1464 of yacc.c  */
#line 941 "grammar.y"
    {
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(4) - (5)].str), newstr(");\n"));
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(2) - (5)].str));
  free((yyvsp[(3) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 134:

/* Line 1464 of yacc.c  */
#line 948 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(3) - (4)].str), newstr(");\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
;}
    break;

  case 135:

/* Line 1464 of yacc.c  */
#line 954 "grammar.y"
    {
  report_error("constant");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 136:

/* Line 1464 of yacc.c  */
#line 961 "grammar.y"
    {
  report_warning("be");
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(3) - (4)].str), newstr(");\n"));
  free((yyvsp[(1) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 137:

/* Line 1464 of yacc.c  */
#line 967 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(2) - (3)].str), newstr(");\n"));
  free((yyvsp[(1) - (3)].str));
;}
    break;

  case 138:

/* Line 1464 of yacc.c  */
#line 972 "grammar.y"
    {
  report_error("constant without article");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (3)].str));
  free((yyvsp[(3) - (3)].str));
;}
    break;

  case 139:

/* Line 1464 of yacc.c  */
#line 978 "grammar.y"
    {
  report_warning("period or exclamation mark");
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(4) - (5)].str), newstr(");\n"));
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(2) - (5)].str));
  free((yyvsp[(3) - (5)].str));
;}
    break;

  case 140:

/* Line 1464 of yacc.c  */
#line 985 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(2) - (5)].str));
  free((yyvsp[(3) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 141:

/* Line 1464 of yacc.c  */
#line 993 "grammar.y"
    {
  report_warning("equality");
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(4) - (5)].str), newstr(");\n"));
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(2) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 142:

/* Line 1464 of yacc.c  */
#line 1000 "grammar.y"
    {
  report_warning("be");
  (yyval.str) = cat5(newstr("assign("), int2str(yylineno), newstr(", second_person, "), (yyvsp[(4) - (5)].str), newstr(");\n"));
  free((yyvsp[(1) - (5)].str));
  free((yyvsp[(3) - (5)].str));
  free((yyvsp[(5) - (5)].str));
;}
    break;

  case 143:

/* Line 1464 of yacc.c  */
#line 1009 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 144:

/* Line 1464 of yacc.c  */
#line 1012 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 145:

/* Line 1464 of yacc.c  */
#line 1017 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 146:

/* Line 1464 of yacc.c  */
#line 1020 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].str), newstr(" "), (yyvsp[(2) - (2)].str));
;}
    break;

  case 147:

/* Line 1464 of yacc.c  */
#line 1024 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 148:

/* Line 1464 of yacc.c  */
#line 1025 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 149:

/* Line 1464 of yacc.c  */
#line 1026 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 150:

/* Line 1464 of yacc.c  */
#line 1027 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 151:

/* Line 1464 of yacc.c  */
#line 1028 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 152:

/* Line 1464 of yacc.c  */
#line 1029 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 153:

/* Line 1464 of yacc.c  */
#line 1030 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 154:

/* Line 1464 of yacc.c  */
#line 1031 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 155:

/* Line 1464 of yacc.c  */
#line 1032 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 156:

/* Line 1464 of yacc.c  */
#line 1033 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 157:

/* Line 1464 of yacc.c  */
#line 1034 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 158:

/* Line 1464 of yacc.c  */
#line 1035 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 159:

/* Line 1464 of yacc.c  */
#line 1036 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 160:

/* Line 1464 of yacc.c  */
#line 1037 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 161:

/* Line 1464 of yacc.c  */
#line 1038 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 162:

/* Line 1464 of yacc.c  */
#line 1039 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 163:

/* Line 1464 of yacc.c  */
#line 1040 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 164:

/* Line 1464 of yacc.c  */
#line 1041 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 165:

/* Line 1464 of yacc.c  */
#line 1042 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 166:

/* Line 1464 of yacc.c  */
#line 1044 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 167:

/* Line 1464 of yacc.c  */
#line 1046 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 168:

/* Line 1464 of yacc.c  */
#line 1047 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 169:

/* Line 1464 of yacc.c  */
#line 1048 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 170:

/* Line 1464 of yacc.c  */
#line 1049 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 171:

/* Line 1464 of yacc.c  */
#line 1050 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 172:

/* Line 1464 of yacc.c  */
#line 1051 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 173:

/* Line 1464 of yacc.c  */
#line 1052 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 174:

/* Line 1464 of yacc.c  */
#line 1053 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 175:

/* Line 1464 of yacc.c  */
#line 1054 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 176:

/* Line 1464 of yacc.c  */
#line 1055 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 177:

/* Line 1464 of yacc.c  */
#line 1056 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 178:

/* Line 1464 of yacc.c  */
#line 1057 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 179:

/* Line 1464 of yacc.c  */
#line 1058 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 180:

/* Line 1464 of yacc.c  */
#line 1059 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 181:

/* Line 1464 of yacc.c  */
#line 1060 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 182:

/* Line 1464 of yacc.c  */
#line 1061 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 183:

/* Line 1464 of yacc.c  */
#line 1062 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 184:

/* Line 1464 of yacc.c  */
#line 1063 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 185:

/* Line 1464 of yacc.c  */
#line 1064 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 186:

/* Line 1464 of yacc.c  */
#line 1065 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 187:

/* Line 1464 of yacc.c  */
#line 1066 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 188:

/* Line 1464 of yacc.c  */
#line 1067 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 189:

/* Line 1464 of yacc.c  */
#line 1068 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 190:

/* Line 1464 of yacc.c  */
#line 1069 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 191:

/* Line 1464 of yacc.c  */
#line 1070 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 192:

/* Line 1464 of yacc.c  */
#line 1071 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 193:

/* Line 1464 of yacc.c  */
#line 1072 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 194:

/* Line 1464 of yacc.c  */
#line 1073 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 195:

/* Line 1464 of yacc.c  */
#line 1074 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 196:

/* Line 1464 of yacc.c  */
#line 1075 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 197:

/* Line 1464 of yacc.c  */
#line 1076 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 198:

/* Line 1464 of yacc.c  */
#line 1077 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 199:

/* Line 1464 of yacc.c  */
#line 1078 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 200:

/* Line 1464 of yacc.c  */
#line 1080 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 201:

/* Line 1464 of yacc.c  */
#line 1081 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 202:

/* Line 1464 of yacc.c  */
#line 1082 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 203:

/* Line 1464 of yacc.c  */
#line 1084 "grammar.y"
    { (yyval.str) = (yyvsp[(1) - (1)].str); ;}
    break;

  case 204:

/* Line 1464 of yacc.c  */
#line 1088 "grammar.y"
    {
  (yyval.str) = cat3(newstr("/******************************************************************\n * "),
	    strpad(str2upper((yyvsp[(1) - (2)].str)), 63, ' '),
	    newstr("*\n ******************************************************************/\n"));
  free((yyvsp[(2) - (2)].str));
;}
    break;

  case 205:

/* Line 1464 of yacc.c  */
#line 1096 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 206:

/* Line 1464 of yacc.c  */
#line 1099 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 207:

/* Line 1464 of yacc.c  */
#line 1104 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_cube("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 208:

/* Line 1464 of yacc.c  */
#line 1110 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_factorial("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 209:

/* Line 1464 of yacc.c  */
#line 1116 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_square("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 210:

/* Line 1464 of yacc.c  */
#line 1122 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_sqrt("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 211:

/* Line 1464 of yacc.c  */
#line 1128 "grammar.y"
    {
  (yyval.stringlist).list = (char **) malloc(2*sizeof(char **));
  (yyval.stringlist).list[0] = cat3(newstr("int_twice("), int2str(yylineno), newstr(", "));
  (yyval.stringlist).list[1] = newstr(")");
  free((yyvsp[(1) - (1)].str));
;}
    break;

  case 212:

/* Line 1464 of yacc.c  */
#line 1136 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 213:

/* Line 1464 of yacc.c  */
#line 1139 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 214:

/* Line 1464 of yacc.c  */
#line 1142 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 215:

/* Line 1464 of yacc.c  */
#line 1145 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 216:

/* Line 1464 of yacc.c  */
#line 1148 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 217:

/* Line 1464 of yacc.c  */
#line 1151 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 218:

/* Line 1464 of yacc.c  */
#line 1156 "grammar.y"
    {
  (yyval.str) = cat2(str2varname((yyvsp[(1) - (1)].str)), newstr("->value"));
;}
    break;

  case 219:

/* Line 1464 of yacc.c  */
#line 1159 "grammar.y"
    {
  (yyval.str) = (yyvsp[(1) - (1)].str);
;}
    break;

  case 220:

/* Line 1464 of yacc.c  */
#line 1162 "grammar.y"
    {
  (yyval.str) = cat5(newstr("value_of("), int2str(yylineno), newstr(", "), (yyvsp[(1) - (1)].str), newstr(")"));
;}
    break;

  case 221:

/* Line 1464 of yacc.c  */
#line 1165 "grammar.y"
    {
  (yyval.str) = cat5((yyvsp[(1) - (4)].stringlist).list[0], (yyvsp[(2) - (4)].str), (yyvsp[(1) - (4)].stringlist).list[1], (yyvsp[(4) - (4)].str), (yyvsp[(1) - (4)].stringlist).list[2]);
  free((yyvsp[(1) - (4)].stringlist).list);
  free((yyvsp[(3) - (4)].str));
;}
    break;

  case 222:

/* Line 1464 of yacc.c  */
#line 1170 "grammar.y"
    {
  (yyval.str) = cat3((yyvsp[(1) - (2)].stringlist).list[0], (yyvsp[(2) - (2)].str), (yyvsp[(1) - (2)].stringlist).list[1]);
  free((yyvsp[(1) - (2)].stringlist).list);
;}
    break;

  case 223:

/* Line 1464 of yacc.c  */
#line 1174 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (4)].stringlist).list[0]);
  free((yyvsp[(1) - (4)].stringlist).list[1]);
  free((yyvsp[(1) - (4)].stringlist).list[2]);
  free((yyvsp[(1) - (4)].stringlist).list);
  free((yyvsp[(2) - (4)].str));
  free((yyvsp[(3) - (4)].str));
;}
    break;

  case 224:

/* Line 1464 of yacc.c  */
#line 1184 "grammar.y"
    {
  report_warning("'and'");
  (yyval.str) = cat5((yyvsp[(1) - (4)].stringlist).list[0], (yyvsp[(2) - (4)].str), (yyvsp[(1) - (4)].stringlist).list[1], (yyvsp[(4) - (4)].str), (yyvsp[(1) - (4)].stringlist).list[2]);
  free((yyvsp[(1) - (4)].stringlist).list);
;}
    break;

  case 225:

/* Line 1464 of yacc.c  */
#line 1189 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (4)].stringlist).list[0]);
  free((yyvsp[(1) - (4)].stringlist).list[1]);
  free((yyvsp[(1) - (4)].stringlist).list[2]);
  free((yyvsp[(1) - (4)].stringlist).list);
  free((yyvsp[(3) - (4)].str));
  free((yyvsp[(4) - (4)].str));
;}
    break;

  case 226:

/* Line 1464 of yacc.c  */
#line 1199 "grammar.y"
    {
  report_error("value");
  (yyval.str) = newstr("");
  free((yyvsp[(1) - (2)].stringlist).list[0]);
  free((yyvsp[(1) - (2)].stringlist).list[1]);
  free((yyvsp[(1) - (2)].stringlist).list);
;}
    break;



/* Line 1464 of yacc.c  */
#line 4194 "grammar.tab.c"
      default: break;
    }
  YY_SYMBOL_PRINT ("-> $$ =", yyr1[yyn], &yyval, &yyloc);

  YYPOPSTACK (yylen);
  yylen = 0;
  YY_STACK_PRINT (yyss, yyssp);

  *++yyvsp = yyval;

  /* Now `shift' the result of the reduction.  Determine what state
     that goes to, based on the state we popped back to and the rule
     number reduced by.  */

  yyn = yyr1[yyn];

  yystate = yypgoto[yyn - YYNTOKENS] + *yyssp;
  if (0 <= yystate && yystate <= YYLAST && yycheck[yystate] == *yyssp)
    yystate = yytable[yystate];
  else
    yystate = yydefgoto[yyn - YYNTOKENS];

  goto yynewstate;


/*------------------------------------.
| yyerrlab -- here on detecting error |
`------------------------------------*/
yyerrlab:
  /* If not already recovering from an error, report this error.  */
  if (!yyerrstatus)
    {
      ++yynerrs;
#if ! YYERROR_VERBOSE
      yyerror (YY_("syntax error"));
#else
      {
	YYSIZE_T yysize = yysyntax_error (0, yystate, yychar);
	if (yymsg_alloc < yysize && yymsg_alloc < YYSTACK_ALLOC_MAXIMUM)
	  {
	    YYSIZE_T yyalloc = 2 * yysize;
	    if (! (yysize <= yyalloc && yyalloc <= YYSTACK_ALLOC_MAXIMUM))
	      yyalloc = YYSTACK_ALLOC_MAXIMUM;
	    if (yymsg != yymsgbuf)
	      YYSTACK_FREE (yymsg);
	    yymsg = (char *) YYSTACK_ALLOC (yyalloc);
	    if (yymsg)
	      yymsg_alloc = yyalloc;
	    else
	      {
		yymsg = yymsgbuf;
		yymsg_alloc = sizeof yymsgbuf;
	      }
	  }

	if (0 < yysize && yysize <= yymsg_alloc)
	  {
	    (void) yysyntax_error (yymsg, yystate, yychar);
	    yyerror (yymsg);
	  }
	else
	  {
	    yyerror (YY_("syntax error"));
	    if (yysize != 0)
	      goto yyexhaustedlab;
	  }
      }
#endif
    }



  if (yyerrstatus == 3)
    {
      /* If just tried and failed to reuse lookahead token after an
	 error, discard it.  */

      if (yychar <= YYEOF)
	{
	  /* Return failure if at end of input.  */
	  if (yychar == YYEOF)
	    YYABORT;
	}
      else
	{
	  yydestruct ("Error: discarding",
		      yytoken, &yylval);
	  yychar = YYEMPTY;
	}
    }

  /* Else will try to reuse lookahead token after shifting the error
     token.  */
  goto yyerrlab1;


/*---------------------------------------------------.
| yyerrorlab -- error raised explicitly by YYERROR.  |
`---------------------------------------------------*/
yyerrorlab:

  /* Pacify compilers like GCC when the user code never invokes
     YYERROR and the label yyerrorlab therefore never appears in user
     code.  */
  if (/*CONSTCOND*/ 0)
     goto yyerrorlab;

  /* Do not reclaim the symbols of the rule which action triggered
     this YYERROR.  */
  YYPOPSTACK (yylen);
  yylen = 0;
  YY_STACK_PRINT (yyss, yyssp);
  yystate = *yyssp;
  goto yyerrlab1;


/*-------------------------------------------------------------.
| yyerrlab1 -- common code for both syntax error and YYERROR.  |
`-------------------------------------------------------------*/
yyerrlab1:
  yyerrstatus = 3;	/* Each real token shifted decrements this.  */

  for (;;)
    {
      yyn = yypact[yystate];
      if (yyn != YYPACT_NINF)
	{
	  yyn += YYTERROR;
	  if (0 <= yyn && yyn <= YYLAST && yycheck[yyn] == YYTERROR)
	    {
	      yyn = yytable[yyn];
	      if (0 < yyn)
		break;
	    }
	}

      /* Pop the current state because it cannot handle the error token.  */
      if (yyssp == yyss)
	YYABORT;


      yydestruct ("Error: popping",
		  yystos[yystate], yyvsp);
      YYPOPSTACK (1);
      yystate = *yyssp;
      YY_STACK_PRINT (yyss, yyssp);
    }

  *++yyvsp = yylval;


  /* Shift the error token.  */
  YY_SYMBOL_PRINT ("Shifting", yystos[yyn], yyvsp, yylsp);

  yystate = yyn;
  goto yynewstate;


/*-------------------------------------.
| yyacceptlab -- YYACCEPT comes here.  |
`-------------------------------------*/
yyacceptlab:
  yyresult = 0;
  goto yyreturn;

/*-----------------------------------.
| yyabortlab -- YYABORT comes here.  |
`-----------------------------------*/
yyabortlab:
  yyresult = 1;
  goto yyreturn;

#if !defined(yyoverflow) || YYERROR_VERBOSE
/*-------------------------------------------------.
| yyexhaustedlab -- memory exhaustion comes here.  |
`-------------------------------------------------*/
yyexhaustedlab:
  yyerror (YY_("memory exhausted"));
  yyresult = 2;
  /* Fall through.  */
#endif

yyreturn:
  if (yychar != YYEMPTY)
     yydestruct ("Cleanup: discarding lookahead",
		 yytoken, &yylval);
  /* Do not reclaim the symbols of the rule which action triggered
     this YYABORT or YYACCEPT.  */
  YYPOPSTACK (yylen);
  YY_STACK_PRINT (yyss, yyssp);
  while (yyssp != yyss)
    {
      yydestruct ("Cleanup: popping",
		  yystos[*yyssp], yyvsp);
      YYPOPSTACK (1);
    }
#ifndef yyoverflow
  if (yyss != yyssa)
    YYSTACK_FREE (yyss);
#endif
#if YYERROR_VERBOSE
  if (yymsg != yymsgbuf)
    YYSTACK_FREE (yymsg);
#endif
  /* Make sure YYID is used.  */
  return YYID (yyresult);
}



/* Line 1684 of yacc.c  */
#line 1207 "grammar.y"


int yyerror(char *s)
{
  /* Don't do anything special here. */
  return 0;
}

void report_error(const char *expected_symbol)
{
  fprintf(stderr, "Error at line %d: %s expected\n", yylineno, expected_symbol);
  num_errors++;
}

void report_warning(const char *expected_symbol)
{
  fprintf(stderr, "Warning at line %d: %s expected\n", yylineno, expected_symbol);
  num_warnings++;
}

int main(void)
{
#if(YYDEBUG == 1)
  yydebug = 1;
#endif
  if (yyparse() == 0) {
    if (num_errors > 0) {
      fprintf(stderr, "%d errors and %d warnings found. No code output.\n", num_errors, num_warnings);
      exit(1);
    } else if (num_warnings > 0) {
      fprintf(stderr, "%d warnings found. Code may be defective.\n", num_warnings);
    }
  } else {
      fprintf(stderr, "Unrecognized error encountered. No code output.\n");
      exit(1);
  }
  return 0;
}

