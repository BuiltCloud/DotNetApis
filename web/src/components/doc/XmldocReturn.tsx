import * as React from "react";

import { Xmldoc } from "./Xmldoc";

import { PackageDoc, IXmldoc } from "../../util";

interface XmldocReturnProps {
    data: IXmldoc;
    pkg: PackageDoc;
}

export const XmldocReturn: React.StatelessComponent<XmldocReturnProps> = ({ data, pkg }) => {
    if (!data || !data.r)
        return null;
    return (
        <div>
            <h2>Return Value</h2>
            <Xmldoc data={data.r} pkg={pkg}/>)
        </div>
    );
}