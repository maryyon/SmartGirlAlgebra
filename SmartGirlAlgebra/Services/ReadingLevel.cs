using System.Text;

namespace SmartGirlAlgebra.Services;

/// <summary>
/// Decides which words a second grader is likely to be able to read.
///
/// The list below is what she is expected to know by the end of second grade:
/// the Dolch sight words through third grade, the Dolch picture nouns, number
/// and math words, and the everyday nouns this version actually uses. Anything
/// NOT on it counts as above her reading level and becomes tappable.
///
/// Erring toward "hard" is deliberate. A word she can already read that gets
/// underlined costs her one curious tap; a word she cannot read that is left
/// plain stops her dead.
/// </summary>
public static class ReadingLevel
{
    private const string Easy = """
        a about after again all always am an and any are around as ask at ate away
        be because been before began being best better big black blue both bring
        brown but buy by call came can carry clean cold come could cut
        did do does done down draw drink
        eat eight every
        fall far fast find first five fly for found four from full funny
        gave get give go goes going gone good got green grow
        had has have he help her here hers him his hold hot how hurt
        i if in into is it its
        jump just
        keep kind know
        laugh let light like little live long look
        made make many may me much must my myself
        name never new next nine no not now
        of off old on once one only open or other our out over own
        pick play please pretty pull put
        ran read red ride right round run
        said saw say see seven shall she show sing sit six sleep small so some
        soon start stop
        take tell ten thank that the their them then there these they think this
        those three to today together too took try two
        under up upon us use
        very
        walk want warm was wash we well went were what when where which while white
        who why will wish with work would write
        yellow yes you your
        apple baby back ball bear bed bell bird birthday boat box boy bread brother
        cake car cat chair chicken children coat corn cow day dog doll door duck egg
        eye farm farmer father feet fire fish floor flower game garden girl good-bye
        grass ground hand head hill home horse house kitty leg letter man men milk
        money morning mother name nest night paper party picture pig rabbit rain ring
        robin santa school seed sheep shoe sister snow song squirrel stick street sun
        table thing time top toy tree watch water way wind window wood
        add answer both count counting each equal equally even extra fair fewer group
        groups half halves less least lot lots match matches matching more most number
        numbers odd pair pairs part parts pile piles plus same share shared shares
        sharing size sizes skip skipping some sum take takes taking total totals
        zero eleven twelve thirteen fourteen fifteen sixteen seventeen eighteen
        nineteen twenty thirty forty fifty sixty seventy eighty ninety hundred
        first second third fourth fifth twos fives tens
        row rows line lines column columns across down chart board
        team teams coach dance dancer dancers dancing sing singer singers singing
        song songs cheer cheers cheering squad squads floor stage show shows
        girls friend friends partner partners bow bows ribbon ribbons medal medals
        sticker stickers candy candies pom poms chair chairs sheet sheets sheet
        spin spins twirl twirls routine practice hair ties tie
        big small hard easy fast slow long short new old right wrong left over
        again next last more less than that this here there when then now
        yes no not never always maybe only just still also
        say says said tell told ask asks asked look looks looked see sees seen
        find finds found get gets got give gives gave put puts hand hands handed
        need needs needed want wants wanted try tries tried keep keeps kept
        does did done make makes made think thinks thought
        picture pictures word words read reads reading
        stand stands standing sit sits sitting turn turns turned
        end ends ending finish finishes begin begins
        one two three four five six seven eight nine ten
        me you her him them us it he she they we i
        almost also always amount another answer any anyone anything around
        become began begin behind below beside between body book bring brought
        build built cannot care careful catch caught certain chance change check
        choose class clear close cold color coming corner crawl cross cry
        deep depend depends does done draw dream drop during
        each early easy edge else empty end enough even ever every everyone
        everything except eyes
        face fact fair fall family fear feel feels fell felt few field fill
        finally find fine finish flat follow food foot force forget form free
        fresh friday friend front
        gone grade great group grow guess
        half happen happy hard head hear heard heavy held hello high hold hole
        hope hour huge hurry
        idea inside instead
        join keep kept kind knew know known
        land large last late learn leave leaves left less let level lie life
        lift light listen live load lone loud love low lucky
        main mark math matter maybe mean means meant meet middle might mind
        minute miss moment move music
        near neat need neither nice noise none note nothing notice
        ocean often once order other outside
        pass past pay perfect person place plain plan point poor power press
        problem pull push
        quick quiet quite
        race raise reach ready real reason remember rest return rich rock roll
        rule rush
        safe save scale scene sense sent set several shape sharp shop short
        should shout side sign silly simple since single skip slow smart smile
        sold solve someone something sometimes soon sorry sound space speak
        spend spirit spot spread stand state stay step still stone stop store
        story straight strange street strong stuck study such sudden sure
        sweet swim
        teach test thick thin third those though thought throw tiny tired
        touch toward trick tricky trip true turn twice
        understand until unless usual
        visit voice
        wait wake watch wave weak wear week weight wide wild win wonder word
        world worry worth write written wrong
        year young
        brain bright build carry catch cheer clean climb count course cover
        crowd dance dark decide desk dress drive drop dry
        different either everybody nobody through doing whole myself herself
        himself itself ourselves upon while without inside outside across
        """;

    private static readonly HashSet<string> Known = Build();

    private static HashSet<string> Build()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var w in Easy.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
        {
            set.Add(w.Trim());
        }

        return set;
    }

    /// <summary>
    /// True when the word is above a second-grade reading level, so it should be
    /// tappable. Numerals, symbols and very short words are never flagged.
    /// </summary>
    public static bool IsHard(string token)
    {
        var word = Strip(token);

        if (word.Length == 0) return false;

        // Numbers and anything with a digit in it read themselves.
        if (word.Any(char.IsDigit)) return false;

        // Two and three letter words are not the problem.
        if (word.Length <= 3) return false;

        return !IsKnown(word);
    }

    private static bool IsKnown(string word)
    {
        if (Known.Contains(word)) return true;

        // Try the obvious endings before giving up, so the list stays short and
        // "jumping", "jumped" and "jumps" all ride on "jump".
        foreach (var stem in Stems(word))
        {
            if (Known.Contains(stem)) return true;
        }

        // Two known words stuck together is still readable at this level:
        // "pom-poms", "good-bye", "cheerleader" is not, but "cannot" is.
        var dash = word.IndexOf('-');
        if (dash > 0 && dash < word.Length - 1)
        {
            var a = word[..dash];
            var b = word[(dash + 1)..];
            if (IsKnown(a) && IsKnown(b)) return true;
        }

        return false;
    }

    private static IEnumerable<string> Stems(string w)
    {
        // plural / third person
        if (w.EndsWith("es") && w.Length > 4)
        {
            yield return w[..^2];
            yield return w[..^2] + "e";
            if (w.EndsWith("ies")) yield return w[..^3] + "y";
        }

        if (w.EndsWith('s') && w.Length > 3) yield return w[..^1];

        // past tense
        if (w.EndsWith("ed") && w.Length > 4)
        {
            yield return w[..^2];
            yield return w[..^1];
            if (w.EndsWith("ied")) yield return w[..^3] + "y";
            yield return Undouble(w[..^2]);
        }

        // continuous
        if (w.EndsWith("ing") && w.Length > 4)
        {
            yield return w[..^3];
            yield return w[..^3] + "e";
            yield return Undouble(w[..^3]);
        }

        // comparatives, including easy -> easier -> easiest
        if (w.EndsWith("er") && w.Length > 4)
        {
            yield return w[..^2];
            yield return Undouble(w[..^2]);
            if (w.EndsWith("ier")) yield return w[..^3] + "y";
        }

        if (w.EndsWith("est") && w.Length > 5)
        {
            yield return w[..^3];
            yield return Undouble(w[..^3]);
            if (w.EndsWith("iest")) yield return w[..^4] + "y";
        }

        // slowly -> slow, happily -> happy
        if (w.EndsWith("ly") && w.Length > 4)
        {
            yield return w[..^2];
            if (w.EndsWith("ily")) yield return w[..^3] + "y";
        }

        // A contraction is only as hard as the word it starts with:
        // we'll, you've, didn't all read fine at this level.
        var tick = w.IndexOfAny(['\'', '’']);
        if (tick > 1)
        {
            yield return w[..tick];

            // didn't -> did, wasn't -> was, couldn't -> could
            if (tick > 2 && w[tick - 1] == 'n') yield return w[..(tick - 1)];
        }
    }

    /// <summary>running -> run, bigger -> big</summary>
    private static string Undouble(string w)
    {
        if (w.Length >= 3 && w[^1] == w[^2] && !"aeiou".Contains(w[^1])) return w[..^1];
        return w;
    }

    /// <summary>Leaves letters, apostrophes and hyphens; drops punctuation and emoji.</summary>
    public static string Strip(string token)
    {
        var sb = new StringBuilder(token.Length);

        for (var i = 0; i < token.Length; i++)
        {
            var c = token[i];

            if (char.IsHighSurrogate(c)) { i++; continue; }
            if (char.IsLowSurrogate(c)) continue;

            if (char.IsLetterOrDigit(c) || c is '\'' or '’' or '-') sb.Append(c);
        }

        return sb.ToString().Trim('-', '\'', '’').ToLowerInvariant();
    }
}
