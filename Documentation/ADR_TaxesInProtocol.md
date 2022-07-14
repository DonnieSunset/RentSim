# Semantics of Taxes in Protocol
## Context and Problem Statement

Interests are clear:

| Total Year begin | Deposits stocks | Interests stocks | Total Year End |
| --- |--- | --- | --- |
| 1.000 | 100 | 20 | 1.120 |

The interests are just added to the total amounts. However with taxes, two interpretations are possible:

## Considered Options

Possibility A) -> Deposits **do include** taxes

| Total Year begin | Deposits stocks | Taxes stocks | Total Year End |
| --- |--- | --- | --- |
| 1.000 | -100 | -5 | 900 |

Pros:
- nothing

Possibility B) -> Deposits **do not include** taxes

| Total Year begin | Deposits stocks | Taxes stocks | Total Year End |
| --- |--- | --- | --- |
| 1.000 | -95 | -5 | 900 |

Pros:
- Sum of all columns give the Total Year End mathematically
- Deposits reflect the net rate, which is relevant for the monthly needs


## Decision Outcome

Possibility B)

