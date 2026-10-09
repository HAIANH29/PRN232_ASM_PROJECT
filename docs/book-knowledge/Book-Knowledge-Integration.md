# Book Knowledge Integration

This project uses book-derived knowledge as curated educational content. Do not commit the full book, scanned pages, copied chapters, or long passages.

## Required Workflow

1. Use a legitimately obtained copy of *The Longevity Diet* for project research.
2. Identify only the chapters/sections needed for diet guidelines, foods, recipes, meal rules, and lifestyle guidance used by this app.
3. Summarize each selected idea in the team's own words.
4. Record source metadata on each managed content item:
   - `SourceTitle`
   - `SourceChapter`
   - `SourcePage` or `SourceReference`
   - `ReviewedBy`
5. Have at least one teammate verify the summary against the source.
6. Mark the item `Approved` only after the source metadata is filled and the review is complete.

## Review Statuses

- `NeedsReview`: default for seed/imported content. It is visible to admins but not returned by public user endpoints.
- `Approved`: reviewed content that can be used by public browsing, Meal Planning, and future Recommendation context.
- `Rejected`: content that should stay out of user-facing and AI context flows.

## Current Implementation

- Diet guidelines, foods, and recipes now store source and review metadata.
- Public Diet Knowledge endpoints return only active `Approved` content.
- Admin endpoints can list all statuses and filter by `reviewStatus`.
- Admin create/update accepts `NeedsReview`, `Approved`, or `Rejected`.
- `Approved` content must include `SourceChapter`, either `SourcePage` or `SourceReference`, and `ReviewedBy`.
- The JSON seed file is version-controlled at `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/Seed/Data/book-knowledge-seed.json`.
- Seed entries are intentionally `NeedsReview` until the team verifies exact source locations from its legal copy.

## AI Safety Rule

Recommendation context must be built only from active `Approved` knowledge plus user preferences. It must not ask Gemini to invent medical advice, diagnosis, treatment, disease prediction, or lifespan prediction.

## Team TODO

- Confirm the legal project source copy exists.
- Fill `SourceChapter`, `SourcePage` or `SourceReference`, and `ReviewedBy` for each seed item.
- Change verified items to `Approved`.
- Keep summaries concise and in the team's own words.
