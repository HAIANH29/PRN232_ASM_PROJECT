# Book Knowledge Integration

This project uses book-derived knowledge as curated educational content. Do not commit the full book, scanned pages, copied chapters, or long passages.

## Required Workflow

1. Use a legitimately obtained copy of *The Longevity Diet* for project research.
2. Admin may upload the team's PDF copy in the Web admin area for private project extraction.
3. Diet Knowledge Service extracts text into Admin-only chunks and can ask Gemini to create knowledge candidates.
4. Identify only the chapters/sections needed for diet guidelines, foods, recipes, meal rules, and lifestyle guidance used by this app.
5. Summarize each selected idea in the team's own words.
6. Record source metadata on each managed content item:
   - `SourceTitle`
   - `SourceChapter`
   - `SourcePage` or `SourceReference`
   - `ReviewedBy`
7. Have at least one teammate verify the summary against the source.
8. Mark the item `Approved` only after the source metadata is filled and the review is complete.

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
- Admin can upload PDF sources from `Admin > Book Sources`.
- Uploaded source metadata, extracted chunks, and generated candidates are stored in `DietKnowledgeDb`.
- Raw extracted chunks are Admin-only and are not returned by public Diet Knowledge endpoints.
- Gemini candidate extraction is controlled by `GEMINI_ENABLED` and `GEMINI_API_KEY`. When Gemini is disabled, the app creates safe review-placeholder candidates so the ingestion flow remains demonstrable.
- Candidates must be explicitly approved before they create active managed knowledge. Approved guideline candidates become `DietGuideline` records; approved food candidates become `Food` records. Recipe candidates must be recreated through the recipe admin screen so ingredients are reviewed explicitly.
- The JSON seed file is version-controlled at `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/Seed/Data/book-knowledge-seed.json`.
- Seed entries are intentionally `NeedsReview` until the team verifies exact source locations from its legal copy.
- Local teacher-demo data is version-controlled separately at `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/Seed/Data/demo-approved-knowledge-seed.json` and can be disabled with `DietKnowledgeSeed__IncludeDemoApprovedContent=false`.
- The demo-approved file exists so fresh local setup has public browse/search/recommendation data; it does not replace the team's final source verification responsibility.

## AI Safety Rule

Recommendation context is built only from active `Approved` knowledge returned by the public Diet Knowledge APIs plus sanitized user preferences. It must not ask Gemini to invent medical advice, diagnosis, treatment, disease prediction, or lifespan prediction.

Admin PDF extraction may use Gemini to summarize chunks, but Recommendation Service must never use uploaded raw PDF text or unapproved candidates directly.

## Uploaded PDF Storage

- Docker stores uploaded PDFs in the `diet-knowledge-book-sources` volume mounted at `/app/storage/book-sources`.
- Local non-Docker runs store files under the Diet Knowledge app base directory by default.
- Do not commit uploaded PDFs, extracted full-text exports, scanned pages, or long copied passages to the repository.

## Team TODO

- Confirm the legal project source copy exists.
- Upload the source PDF through the Admin Book Sources screen if the team wants AI-assisted candidate extraction.
- Fill `SourceChapter`, `SourcePage` or `SourceReference`, and `ReviewedBy` for each seed item.
- Change verified items to `Approved`.
- Keep summaries concise and in the team's own words.
