# dotnet-api-template

**Template** for an ASP.NET Core (.NET 10) REST API with Swagger, tests, code coverage, Trivy scanning and a container image
pipeline. Create a project from it (*New project > Create from template > Group*) instead of copying files.

| What you get | |
|--------------|---|
| `src/App` | ASP.NET Core controllers, **Swagger UI** at `/swagger`, `GET /health`, `GET /api/info` (build, environment), a sample `GET /api/hello` |
| `tests/App.Tests` | xUnit: a unit test and integration tests through the real HTTP pipeline (`WebApplicationFactory`) |
| `.gitlab-ci.yml` | build (locked restore), test with **coverage** (Cobertura in merge requests, HTML artifact), **Trivy** on the repository and the image, `docker build` on Microsoft's ASP.NET Core image, push to the Container Registry |
| `Dockerfile` | `mcr.microsoft.com/dotnet/aspnet:10.0`, non-root user, the published application only |
| `deploy/` | the scripts the deploy jobs use (`docker login` with the deploy token, pull, restart the systemd unit) |

## After you create a project from it

1. Rename `App` if you like (solution, projects, namespace, the `App.dll` in the `Dockerfile` and `build` job).
2. Run `dotnet restore` once and commit the updated `packages.lock.json` files.
3. The pipeline builds, tests and scans everything, and pushes `registry.maresdemo.com/<group>/<project>:<pipeline id>`.

## Deploying

The deploy jobs are dormant (`ENABLE_DEPLOY` is `"false"`) until the project exists on the deploy server:

1. Add the project to the `apps:` list of the **ansible** project (name = project name, a free port pair, `kind: docker`) and
   run its pipeline: it creates the unit `<project>-<env>`, the directories and the HTTPS sites
   `<project>-staging.examples.maresdemo.com` / `<project>-production.examples.maresdemo.com`.
2. Create a deploy token named **`gitlab-deploy-token`** with the scope `read_registry` (*Settings > Repository > Deploy tokens*).
   GitLab hands it to the pipeline as `CI_DEPLOY_USER` / `CI_DEPLOY_PASSWORD`.
3. Set the CI/CD variable `ENABLE_DEPLOY` to `true`.

`staging` then deploys on every commit to `main`; `production` is a manual job (protect the environment and require
approvals in *Settings > CI/CD > Protected environments*, Premium).
