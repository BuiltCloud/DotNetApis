import { getJson } from "./util";
import { MessageBase } from "./messages";
import { without$ } from "../util";

export type Status = "Requested" | "Succeeded" | "Failed";
export interface StatusResponse extends MessageBase {
    status: Status;
    logUri: string;
    jsonUri: string;
}

export const getStatus = (key: PackageKey) =>
    getJson<StatusResponse>("http://localhost:7071/api/0/status", {...without$(key)});