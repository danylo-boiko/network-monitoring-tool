export function intToIpString(ip: number): string {
  return [24, 16, 8, 0].map(n => (ip >> n) & 0xff).join(".");
}

export function ipStringToInt(ip: string): number {
  return ip.split(".").reduce((sum, x, i) => sum + (parseInt(x) << 8 * (3 - i)), 0);
}

export function isIpStringValid(ip: string): boolean {
  return new RegExp(/\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b/).test(ip);
}
