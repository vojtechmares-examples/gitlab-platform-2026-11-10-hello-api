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

## CI configuration

The build, test and image jobs need no configuration of their own. The deploy jobs read the CI/CD variables below (*Settings > CI/CD > Variables*). Variables that start with `CI_` are predefined by GitLab.

| Variable | Set on | Type | Used for |
|----------|--------|------|----------|
| `DEPLOY_HOST` | group `examples` | Variable | Address of the deploy server |
| `DEPLOY_SSH_PORT` | group `examples` | Variable | SSH port of the deploy server (`6022`) |
| `DEPLOY_USER` | group `examples` | Variable | Account the pipelines log in as (`deploy`) |
| `SSH_KNOWN_HOSTS` | group `examples` | Variable | Host keys of the deploy server; the connection uses `StrictHostKeyChecking yes` |
| `SSH_PRIVATE_KEY` | group `examples` | File, **protected** | Private key of the `deploy` account |
| `EXAMPLES_DOMAIN` | group `examples` | Variable | Parent domain of the applications (`examples.maresdemo.com`); the deploy job waits for `https://<project>-<env>.$EXAMPLES_DOMAIN` |
| `ENABLE_DEPLOY` | your project | Variable | `"false"` in `.gitlab-ci.yml`: the staging, production and rollback jobs do not exist until you set it to `true` as a project variable (a project variable ranks above the one in the file) |
| `CI_DEPLOY_USER`, `CI_DEPLOY_PASSWORD` | deploy token of your project | Predefined, from the token named **`gitlab-deploy-token`** (scope `read_registry`) | `deploy/release.sh` logs in to the Container Registry on the server with it. The name must be exactly `gitlab-deploy-token` |

The group variables are inherited by every project in the `examples` group, so nothing has to be set for them in a project created from this template. The `.ssh-setup` snippet of `examples/ci-templates` turns the first five into `ssh deploy`.
A **protected** variable is passed only to pipelines on protected branches and tags (`main`), which is where the deploy jobs run.
