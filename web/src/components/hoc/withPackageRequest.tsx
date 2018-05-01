import * as React from 'react';

import { RouteComponentProps, Hoc, ExtendingHoc } from '.';
import { State } from '../../reducers';
import { packageKey, PackageDocumentationRequest } from '../../util';

export interface PackageRequestInjectedProps {
    pkgRequestKey: PackageKey;
    pkgRequestStatus: PackageDocumentationRequest;
}
export type PackageRequestRequiredProps = State & RouteComponentProps<PackageKey>;

function createWithPackageRequest<TProps>(): Hoc<TProps & PackageRequestRequiredProps, TProps & PackageRequestRequiredProps & PackageRequestInjectedProps> {
    return Component => props => {
        const request = props.packageDoc.packageDocumentationRequests[packageKey(props.match.params)];
        return <Component {...props} pkgRequestStatus={request} pkgRequestKey={props.match.params}/>;
    };
}

/** Takes the route parameters `PackageKey`, and injects `PackageRequestInjectedProps` */
export const withPackageRequest : ExtendingHoc<PackageRequestInjectedProps, PackageRequestRequiredProps> = createWithPackageRequest();
