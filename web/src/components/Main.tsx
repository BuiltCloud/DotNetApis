import * as React from "react";
import { Route } from "react-router-dom";
import { Grid, Row, Col } from "react-flexbox-grid";

import { Home } from "./Home";
import { Package } from "./Package";
import { EntityDoc } from "./EntityDoc";
import { PackageFile } from "./PackageFile";
import { PackageNamespace } from "./PackageNamespace";
import { PackageLog } from "./PackageLog";

import { State } from "../reducers";
import { Actions } from "../actions";

export function Main(props: State & Actions)
{
    return (
    <div>
        <Grid fluid>
            <Row>
                <Col lg={6} lgOffset={3}>
                    <Route exact path="/" render={() => <Home {...props} />} />
                    <Route exact path="/pkg/:packageId/:packageVersion?/:targetFramework?" render={() => <Package {...props}/>}/>
                    <Route exact path="/pkg/:packageId/:packageVersion/:targetFramework/doc/:dnaid+" render={() => <EntityDoc {...props}/>}/>
                    <Route exact path="/pkg/:packageId/:packageVersion/:targetFramework/file/:path+" render={() => <PackageFile {...props}/>}/>
                    <Route exact path="/pkg/:packageId/:packageVersion/:targetFramework/ns/:ns" render={() => <PackageNamespace {...props}/>}/>
                    <Route exact path="/pkg/:packageId/:packageVersion/:targetFramework/log" render={() => <PackageLog {...props}/>}/>
                </Col>
            </Row>
        </Grid>
    </div>);
}
