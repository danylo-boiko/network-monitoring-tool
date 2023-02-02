export function intToIpString(ip: number): string {
  return [24, 16, 8, 0].map(n => (ip >> n) & 0xff).join(".");
}

export function ipStringToInt(ip: string): number {
  return ip.split(".").reduce((sum, x, i) => sum + (parseInt(x) << 8 * (3 - i)), 0);
}
