# Sober integration

The original repository is upstream. Keep independent library fixes on focused branches; changes
with prerequisites name that prerequisite rather than importing the complete integration branch.
The existing developer Windows fixes are preserved at sober/windows-baseline (38f6d28).

| Branch | Scope / prerequisite |
| --- | --- |
| fix/android-upright-width | Upright width cap and bitmap tests; upstream main |
| feature/enrollment-capture-results | Original accepted analysis JPEG/index/Person ID; upstream main |
| fix/enrollment-run-lifecycle | Frozen runs and cancellation/drain; enrollment-capture-results |
| feature/forget-selected-enrollment-shots | Identity-scoped selected-shot/vector cleanup; upstream main |
| feature/configurable-guide-aspect | Configurable drawing ratio and geometry tests; upstream main |
| feature/enrollment-presentation | Supported overlay opt-out and accepted event; enrollment-run-lifecycle |
| sober/integration | Combined library fixes and existing Windows baseline |
| sober/internal-packaging | Internal build/package configuration on top of integration |

Run tests/Shiny.FaceIntelligence.Tests before integration, then build the combined MAUI library for
Windows/Android and the Apple targets on a supported host. Pure geometry/bitmap tests link the
production source; they do not prove native camera behavior. Native camera checks remain required.

tools/Pack-Sober.ps1 requires an explicit immutable internal version and output folder. It passes
PublicRelease to Nerdbank.GitVersioning and verifies version.json, so all five face packages and dependencies agree.
Publish these packages to the private feed; do not enable the inherited public NuGet workflow.
An own-feed URL/token must be supplied by the owning environment; never commit credentials.

For local integration, the consuming app's repository-relative artifacts/face-packages source is
a bootstrap feed, populated by this script. It is not checked in and is not a substitute for hosted
feed verification. Do not treat a cached restore as a clean-machine hosted-feed test.

No upstream pull requests are created automatically. Each branch remains available for separate
review/submission at the user's request. Application/client IDs, captions, routes and enrollment
policy stay outside the reusable library.
