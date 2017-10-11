import * as React from "react";
import { Tab, ListItem } from "material-ui";

import { HashTabs } from "./HashTabs";
import { FilteredListItem, HashFilteredList } from "./HashFilteredList";
import { PackageEntityLink } from "./links";

import { State } from "../reducers";
import { Actions } from "../actions";
import { withAutoPackage, PackageInjectedProps } from "./hoc";
import { PackageContext } from "../util";
import { IEntity } from "../structure";
import { simpleDeclaration } from "../fragments";
import { packageEntityLink } from "../logic";

export interface PackageProps extends State, Actions {
}

function normalizePath(path: string): string {
    return path.replace(/\\/g, '/');
}

const PackageComponent: React.StatelessComponent<PackageProps & PackageInjectedProps> = props => {
    const { pkg, packageDoc, pkgRequestKey } = props;
    const types = pkg.l.map(x => x.t).reduce((a, b) => a.concat(b), []);
    types.sort((x, y) => {
        if (x.n < y.n)
            return -1;
        if (x.n > y.n)
            return 1;
        return 0;
    });

    // TODO: determine defaultTabValue based on which tabs are visible
    return (
    <HashTabs defaultTabValue="types">
        {typesTab(props, types)}
        {assembliesTab(props, types)}
    </HashTabs>);
}

export const Package = withAutoPackage(PackageComponent);

function typesTab(pkgContext: PackageContext, types: IEntity[]) {
    if (types.length === 0)
        return null;
     // TODO: fix any hack
    const items : FilteredListItem[] = types.map(x => ({
        search: x.n,
        content:
            <PackageEntityLink {...pkgContext.pkgRequestKey} dnaid={x.i} key={x.i}>
                <ListItem><code>{simpleDeclaration(pkgContext, x, (x as any).s)}</code></ListItem>
            </PackageEntityLink>
    }));
    return <Tab label="Types" value="types" key="types"><HashFilteredList items={items} hashPrefix="types" /></Tab>;
}

function assembliesTab(pkgContext: PackageContext, types: IEntity[]) {
    const items : FilteredListItem[] = types.map(x => ({
        search: x.n,
        content: <div key={"bob" + x.i}></div>
    }));
    return <Tab label="Assemblies" value="assemblies" key="assemblies"><HashFilteredList items={items} hashPrefix="assemblies" /></Tab>;
}