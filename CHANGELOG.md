# Changelog

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
