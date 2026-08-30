# Changelog

## 2026-08-30 — /ds: ninth grade, basketball, Algebra 1

- New version at **/ds** — "Smart Full-Court Algebra", ninth grade, basketball.
- A full **Algebra 1** semester on the same generated, no-guessing engine:
  signs and powers → solving equations → inequalities and rates → slope and
  lines → systems and exponents → quadratics and Pythagoras. 24 skills.
- Five play levels at real Algebra 1 level, not the one-step problems the
  younger versions use.
- Colours are a **placeholder** (hardwood/orange/charcoal) until the real brand
  colours arrive.
- Audit: 14,400 generated problems / 45,600 steps across all 48 skills, clean.

## 2026-08-30 — /rz and /vc: a whole semester, and no way to guess

- **The multiple choice is gone.** Every step is now TYPED. Four options can be
  guessed one time in four, and a fixed list of questions gets memorised — she
  was remembering which button was green, not doing the maths.
- **Every problem is generated fresh.** New numbers every time, so there is
  nothing to memorise. 24 skills, unlimited problems.
- **A full semester, starting a grade BELOW her.** Six units: place value and
  carrying (4th) → factors, long division → fractions → decimals → order of
  operations → one- and two-step equations (pre-algebra).
- **Every step must be shown.** She types each intermediate value; the Next
  button stays disabled until it's right; a wrong answer offers a hint, never
  the answer.
- Nothing unlocks until the one before it is worked — three problems each, all
  steps, on different numbers each time.
- Verified: 9,600 generated problems / 30,400 steps structurally clean, and
  22,950 displayed equations checked arithmetically.

## 2026-08-26 — A mute button on every screen, in every version

- The switch that turns off the silly lines and Nana's voice now sits in the
  **header of every screen**, in all eight versions — not just the home page.
  The moment you want it off is the moment it just went off, and having to
  navigate home first is the same as not having a switch.
- Turning it off also **stops whatever is talking right now**, which is usually
  the reason someone reached for it.
- Renamed from "Silly sounds" to **"Nana's voice"**, because that's what it is.
- The setting is remembered, and both switches read the same one.

## 2026-08-26 — Deploys can now be proven, not assumed

- The build stamps the commit into `version.txt` and `buildinfo.json`, both
  served `no-store`.
- Until now SmartGirlAlgebra had no version marker at all, so "the deploy went
  green" was the only evidence the live site was current — and that only proves
  the upload started, not what is being served.
- P3UP's Step 6 check now works here: fetch `/version.txt` and compare it to the
  merged SHA.

## 2026-08-26 — No moving on until it's right, in every version

- The lesson boards for **/kt, /rz and /vc** used to reveal the answer after a
  wrong tap and let her carry on. They don't any more: the wrong choice greys
  out, the question stays open, and **Next stays disabled until she gets it**.
- A wrong answer offers **"Do you want a hint?"** instead of the answer.
- Mastery still measures the clean first answer — no wrong turns, no hint — so
  getting there eventually moves her forward without pretending she had it.
- /sl already worked this way via the walked lessons. The play boards in every
  version already did too: a wrong tap dims and she tries again.

## 2026-08-26 — /sl lessons are walked through, not guessed at

- Every /sl lesson is now **worked step by step**. The story types itself out
  where she can watch it being set up, and nothing is asked until it's done.
- **She cannot move on until the current step is right.** The next button stays
  disabled. No guessing past anything.
- A wrong answer offers **"Do you want a hint?"** rather than the answer. Hints
  come one at a time, gentlest first; the last one gives it away, because being
  stuck teaches nothing either.
- **The groups are drawn.** "5 groups of 2" shows five boxes with two things in
  each, filling in one group at a time as the working goes along; groups still
  to come are dashed outlines so she can see how many are left.
- 5 lessons, 10 worked problems, 30 steps, every step with three hints.
- Points only count fully for a step solved without a hint.
- **Nav buttons**: every page in every version now has both a way back and a way
  home. The lesson boards previously had no way home at all.

## 2026-08-26 — KT's microphone stays on the machine where it can

- The read-back now asks the browser to recognise his voice **on the device**,
  so the audio never leaves the laptop.
- Where the browser can't do that, a **grown-up must tap once per session**
  before the microphone opens, with a plain sentence saying the sound goes to
  the tablet's speech service and nothing is saved.
- Background: the FTC's amended COPPA Rule (in force June 2025, compliance due
  April 2026) makes voiceprints personal information. The narrow exception for
  a child's voice requires immediate deletion — which can't be promised for
  audio handled by someone else's cloud service.

## 2026-08-25 — Jokes sit under the reading, not over it

- The silly lines and tagline now play at **75% volume**; whispers scale from
  that. Volume only — the recordings are untouched, so her voice keeps its tone.
- **Reading always wins.** Asking to be read to, tapping a word, or opening the
  microphone now stops a joke mid-flight. Previously a joke already playing
  would talk over a child being read to.

## 2026-08-25 — It's her voice now

- All 17 silly lines and the English tagline are now **Mary's own voice**
  (ElevenLabs clone "Bea"), not the browser's synthetic one. ~600KB of audio.
- Where no recording exists — Spanish and Patwa, until a voice is chosen — it
  falls back to the synthetic voice rather than going silent.
- Whispering works on recordings too: same clip, played quieter.

## 2026-08-25 — The signature whispers, and speaks three languages

- The tagline is **whispered about one time in three** — real volume control,
  slower and lower, not just quieter wording.
- It now comes in **English, Spanish and Patwa**. English carries roughly seven
  times in ten; the other two turn up now and then.
- A non-English line is only used if the device actually has a voice for that
  language. Otherwise it falls back to English — an English voice reading
  Spanish is worse than plain English, and worse still for Patwa.

## 2026-08-25 — The apps giggle back

- About one button tap in twenty-five, and never twice inside a minute, the app
  says something silly and then Grandma's line: *"Who loves you more than a
  roadtrip in an RV, sweetheart?"* Always the pair.
- Lines are age-matched per version: giggly for /kt, /jd, /sl; wry for /rz,
  /vc, /jq; dry for /layla and /dm. The tagline is identical for everyone.
- It never fires over an open microphone (KT's read-back), never mid-sentence
  over the reading voice, and never off the reading controls or whiteboard.
- **Silly sounds on/off** toggle on every version's home screen; it remembers,
  and switching it back on plays one so you know what you allowed.
- Currently in the browser's own voice. Recorded clips drop in later without
  changing any of the above.

## 2026-08-25 — Reading help across /kt, /jd, /sl, and RZ's whiteboard

- **/kt now reads *with* him, not just to him.** The read-along stops one word
  short. That word flashes, he says it, and the app listens. Right → it says so
  out loud. Wrong, quiet, or no microphone → it reads the sentence again, slows
  down before the word, says it slowly, and repeats it. Never phrased as a miss.
- Any word in /kt can also be tapped to hear it on its own.
- The microphone opens only on his button press and closes as soon as a word
  comes back or ~6s pass. Refused permission is remembered for the session and
  it falls back to teaching him the word.
- **/jd** now reads like /sl — tap any word to hear it.
- **RZ's lesson whiteboard moved above the answers**; under a question plus four
  stacked choices it sat off the bottom of a phone screen.

## 2026-08-25 — Tap-to-hear hard words in /sl

- In `/sl`, any word above a 2nd-grade reading level is now **underlined with a
  dotted line**. Tapping it says that word **slowly**.
- Applies to the problem, the question, the tips, and every lesson screen — a
  tip she can't read is not a tip.
- Only hard words are tappable. Underlining everything would hide the ones she
  actually needs. On her current content that is 6 words across the game and
  lessons (tumbling, gymnasts, cheerleaders, certified, digit, handy), plus the
  scripture names.

## 2026-08-25 — /sl lowered to 2nd-grade math

- Out: times tables to 7 x 8, one-quarter of a group, two-step problems into
  the fifties. Those are 3rd-grade skills.
- In: equal groups as repeated addition, skip counting by 2s/5s/10s, arrays no
  bigger than 5 x 5, fair shares, and odd/even. Still early multiplication and
  division — just at the age it actually starts.
- Lesson 5 changed from "times and divide undo each other" to "odd and even".
- No x or / notation anywhere; 2nd grade meets those as words first.

## 2026-08-25 — Read-aloud for the kindergarten version

- `/kt` now has a **"Read it to me"** button on the game screen and on every
  lesson screen. It says the instructions out loud and **lights up each word as
  it is spoken**, so a child who can't read yet can follow along.
- Emoji are shown but not spoken — a voice reading "dog face dog face dog face"
  is noise, and the pictures already do that job.
- Falls back to paced highlighting on iPads, where Safari never reports word
  boundaries. Slightly out of step beats no highlight at all.

## 2026-08-25 — Whiteboard on every screen

- The whiteboard is now on **every board in every version**: the step-by-step
  solver, the tap-the-answer board (which never had one), and the lesson
  questions (which never had one either).
- **Made it much bigger and easier to spot.** It was a 10rem strip behind a faint
  dashed hairline with a small grey caption; a child looking for somewhere to work
  scrolled straight past it. Now 18rem tall, a solid border in the version's own
  colour, and headed "Whiteboard — work it out here".
- Thicker marker (3.5 → 5) to suit a fingertip on the larger board.

## [Seven versions, one app] - 2026-08-25

One deployment now serves seven audiences. A version is a JSON file under
`SmartGirlAlgebra/wwwroot/content/` plus a line in `profiles.json` — no rebuild,
no developer.

| route | who | grade | subject | board | palette |
|---|---|---|---|---|---|
| `/layla` | girl | 9th | algebra | whiteboard | navy & red |
| `/jd` | girl | 1st | math | tap-answer | lavender |
| `/sl` | girl | 3rd | math | tap-answer | turquoise |
| `/rz` | girl | 5th | math | whiteboard | seven-colour paint box |
| `/vc` | girl | 5th | math | whiteboard | red only |
| `/jq` | boy | 7th | algebra | whiteboard | green & gold |
| `/dm` | boy | 11th | **physics** | whiteboard | aviation navy |

### Added
- **Two boards, chosen by grade.** Under about 4th grade there is no algebraic
  notation, so those versions tap an answer: no keyboard, no equals sign. Older
  versions show their working line by line and every line is checked.
- **Per-version voice.** Encouragement moved out of code and into each version's
  file. A wrong tap on the younger versions used to say nothing at all.
- **Scripture** on each celebration card, pitched by age. World English Bible,
  which is public domain — the copyrighted translations carry quotation limits
  and attribution requirements. Every verse was fetched from source rather than
  quoted from memory, verses rendering the divine name as "Yahweh" were avoided,
  and stiff wording was swapped for a plainer verse rather than reworded.
- **Drifting themed icons**, inline SVG so they take each version's colours.
  Emoji cannot: a lavender version would still have been given an orange trophy.
- **An unknown-version screen.** A mistyped route used to load the default
  silently, so `/vt` showed Layla's app while the address bar said `vt`.
- **Host mapping**, so a domain can open on a version without a path.
- **DM brings a second subject**: physics. Each level is one law, restated in the
  hints and again in the walkthrough.

### Fixed
- The screen now updates before anything touches the network. Recording an
  attempt can wake a sleeping API and a paused database, so a child could tap an
  answer and watch nothing happen for up to a minute.
- Every version's level numerals were stuck on the original red: two CSS rules
  referenced the default tokens instead of the themed ones.
- The phone bezel, shadows and several borders were navy regardless of version.
- The API died with HTTP 500.30 and stayed dead: the free database pauses when
  idle, and the startup migration could not survive the wake-up. Now retries,
  and an asleep database can no longer take the API down.
- A self-removing service worker was still being registered by `index.html`, so
  it reinstalled itself in a loop.

### Technical
- Branch: main
- Backup branch: backup-20260825-1606-seven-versions
- Rollback tag: ROLLBACK-SEVEN-VERSIONS-2026-08-25
- Offline bundle: `Backup/SmartGirlAlgebra-20260825-1606.bundle`
- Hosting: Static Web App (Free) + App Service F1 + Azure SQL free tier — $0/month

---

## [Rebuilt around step-by-step solving] - 2026-08-22

Replaced the single-answer practice screen with a whiteboard that checks every
line of working as it is written, and adopted the blue/red/silver identity
designed in v0 in place of the original hot pink.

### Fixed
- Removed the SQL connection string and JWT key from `appsettings.json`, and
  scrubbed them from git history. They had been public for eight months, in
  `appsettings.json` **and** in a JetBrains AI-assistant transcript committed
  under `.idea/`.
- Deleted two watermarked stock previews that should never have shipped.
- Removed three fabricated five-star testimonials, and a feedback form that
  asked children for their names, had no backend, and promised review.
- The service worker's cache name was hard-coded to a date and never changed, so
  browsers served themselves December's homepage indefinitely.

### Changed
- Accounts replaced by sync codes: no email, no password, no name. Progress
  follows a code like `SGA-7K4M2P` to any device.
- Cost: $61.08/month to $0.00/month.
